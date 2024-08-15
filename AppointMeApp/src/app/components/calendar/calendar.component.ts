import { ChangeDetectorRef, Component, OnInit, signal, ViewChild } from '@angular/core';
import { FullCalendarComponent, FullCalendarModule } from '@fullcalendar/angular'; // Import FullCalendarComponent
import { CalendarOptions, EventApi, EventClickArg, EventInput } from '@fullcalendar/core';
import { MatDialog } from '@angular/material/dialog';
import { EventDialogComponent } from './event-dialog/event-dialog.component';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import listPlugin from '@fullcalendar/list';

import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { BusinessService } from '../../services/business.service';
import { AppointmentsService } from '../../services/appointments.service';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Appointment } from '../../classes/appointment.model';


@Component({
  selector: 'app-calendar',
  standalone: true,
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
  imports: [
    CommonModule,
    FullCalendarModule, 
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    EventDialogComponent
  ]
})
export class CalendarComponent implements OnInit {

  public userName: string = "";
  public eventList: EventInput[] = [];

  constructor(
    public dialog: MatDialog,
    public businessService: BusinessService,
    public appointmentService: AppointmentsService,
    private cdr: ChangeDetectorRef  // Inject ChangeDetectorRef
  ) {}

  @ViewChild(FullCalendarComponent) calendarComponent!: FullCalendarComponent;
  //instance of the calendar with options
  calendarOptions: CalendarOptions = {
    plugins: [
      dayGridPlugin, 
      interactionPlugin, 
      timeGridPlugin, 
      listPlugin],
    headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
      },
    initialView: 'dayGridMonth',
    initialEvents: this.eventList,  // This will be updated dynamically
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    dateClick: this.handleDateClick.bind(this),
    eventClick: this.handleEventClick.bind(this),
    eventsSet: this.handleEvents.bind(this)
  };

  
  ngOnInit(): void {
    //saving username from token
    this.businessService.getUserNameFromStore().subscribe(res => {
      let userNameFromToken = this.businessService.getUserNameFromToken();
      this.userName = res || userNameFromToken;

      //setting events to the eventList[]
      let params = new HttpParams().set('username', this.userName);
      
      this.appointmentService.GetAppointmentsByBusinessUserName(params).subscribe({
        next: (events: EventInput[]) => {
          this.eventList = events;
          
          // Get the calendar API
          const calendarApi = this.calendarComponent.getApi();
      
          // Remove all existing events
          calendarApi.removeAllEvents();
      
          // Add new events
          this.eventList.forEach(event => {
            calendarApi.addEvent(event);
          });
          
          this.cdr.detectChanges();  // Manually trigger change detection
        },
        error: err => console.log(err)
      });
    });
  }

  currentEvents = signal<EventApi[]>([]);


  handleDateClick(arg: any): void {
    
    //calling the event-dialog popup
    const dialogRef = this.dialog.open(EventDialogComponent, {
      width: '400px',
      data: { 
        title: '',
        allDay: false,
        startDate: arg.date,
        startTime: '',
        endDate: arg.date,
        endTime: '',
        description: '',
        customerId: ''
      }
    });
    
    //logic for closing or submitting the popup
    dialogRef.afterClosed().subscribe(result => {
      if (result && this.calendarComponent) {
        const calendarApi = this.calendarComponent.getApi();
  
        const startDate = new Date(result.startDate);
        if (result.startTime) {
          const [hours, minutes] = result.startTime.split(':');
          startDate.setHours(Number(hours), Number(minutes));
        }
        
        // Convert startDate to UTC by creating a new Date object with the same timestamp but in UTC
        const startUTC = new Date(Date.UTC(
          startDate.getUTCFullYear(),
          startDate.getUTCMonth(),
          startDate.getUTCDate(),
          startDate.getUTCHours(),
          startDate.getUTCMinutes(),
          startDate.getUTCSeconds()
        ));

        const endDate = new Date(result.endDate);
        if (result.endTime) {
          const [hours, minutes] = result.endTime.split(':');
          endDate.setHours(Number(hours), Number(minutes));
        }
        
        // Convert endDate to UTC similarly
        const endUTC = new Date(Date.UTC(
          endDate.getUTCFullYear(),
          endDate.getUTCMonth(),
          endDate.getUTCDate(),
          endDate.getUTCHours(),
          endDate.getUTCMinutes(),
          endDate.getUTCSeconds()
        ));

        const customerId = result.customerId; 

        const newAppointment: Appointment = {
          id: 0,
          title: result.title,
          start: startUTC,
          end: endUTC,
          allDay: result.allDay,
          extendedProps: {
            description: result.description || '',  // Ensure description is not undefined
            customerId: result.customerId || 0    // Ensure customerId is not undefined
          },
        };
        
        //adding the Event to the eventlist
        calendarApi.addEvent({
          title: newAppointment.title,
          start: newAppointment.start,
          end: newAppointment.end,
          extendedProps: {
            description: newAppointment.extendedProps?.description,
            customerId: result.customerId
          },
          allDay: newAppointment.allDay
        });
      

        // Send POST request to save the appointment
        let params = new HttpParams().set('username', this.userName).set('customerId', customerId);

        const headers = new HttpHeaders()
        .append(
          'Content-Type',
          'application/json'
        );

        //const appointmentBody=JSON.stringify(newAppointment);
        
        this.appointmentService.createAppointment(newAppointment, params, headers).subscribe({
          next: (response: any) => {
            console.log('Appointment saved:', response);
            newAppointment.id = response.id; // Updating local appointment id with the one from the backend
            
            calendarApi.addEvent({
              id: newAppointment.id, 
              title: newAppointment.title,
              start: newAppointment.start,
              end: newAppointment.end,
              extendedProps: {
                description: newAppointment.extendedProps?.description,
                customerId: newAppointment.extendedProps?.customerId
              },
              allDay: newAppointment.allDay
            });
          },
          error: err => console.error('Error saving appointment:', err)
        });
      } else {
        console.error('FullCalendarComponent instance is not initialized.');
      }
    });
  }

  //clicking on events
  handleEventClick(clickInfo: EventClickArg) {
    const event = clickInfo.event;
    
    const dialogRef = this.dialog.open(EventDialogComponent, {
      width: '400px',
      data: {
        title: event.title,
        allDay: event.allDay,
        startDate: event.start,
        startTime: event.start ? this.formatTime(event.start) : '',
        endDate: event.end,
        endTime: event.end ? this.formatTime(event.end) : '',
        description: event.extendedProps['description'], 
        customerId: event.extendedProps['customerId'],    
        isEdit: true // flag to indicate this is an edit
      }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (result.delete) {
          // Handle deletion
          event.remove();
          this.deleteAppointment(Number(event.id));
        } else {
          // Handle editing
          const startDate = new Date(result.startDate);
          if (result.startTime) {
            const [hours, minutes] = result.startTime.split(':');
            startDate.setHours(Number(hours), Number(minutes));
          }
          
          const endDate = new Date(result.endDate);
          if (result.endTime) {
            const [hours, minutes] = result.endTime.split(':');
            endDate.setHours(Number(hours), Number(minutes));
          }
  
          event.setProp('title', result.title);
          event.setDates(startDate, endDate);
          event.setAllDay(result.allDay);
          event.setExtendedProp('description', result.description);
          event.setExtendedProp('customerId', result.customerId);
          
          console.log(event);
          this.updateAppointment(event); // Update in the backend
        }
      }
    });
  }
  
  // Utility function to format time
  private formatTime(date: Date): string {
    return date.toTimeString().slice(0, 5);
  }
  
  //updating existing Event
  updateAppointment(event: EventApi): void {
    const updatedAppointment: Appointment = {
      id: parseInt(event.id, 10), // Convert string id back to number if necessary
      title: event.title,
      start: event.start as Date,
      end: event.end as Date,
      extendedProps: {
        description: event.extendedProps['description'],
        customerId: event.extendedProps['customerId']
      },
      allDay: event.allDay,
      
    };
  
    const headers = new HttpHeaders().append('Content-Type', 'application/json');
    let params = new HttpParams().set('username', this.userName).set('eventId', event.id);
    
    /*
    this.appointmentService.updateAppointment(JSON.stringify(updatedAppointment), params, headers).subscribe({
      next: response => console.log('Appointment updated:', response),
      error: err => console.error('Error updating appointment:', err)
    });*/
  }
  


  //Deleting Event
  
  deleteAppointment(appointmentId: number): void {
    this.appointmentService.deleteAppointment(appointmentId).subscribe({
      next: () => {
        console.log('Appointment deleted successfully');
      },
      error: err => {
        console.error('Error deleting appointment:', err);
      }
    });
  }

  //handling events
  handleEvents(events: EventApi[]) {
    this.currentEvents.set(events);
    this.cdr.detectChanges(); 
  }
}
