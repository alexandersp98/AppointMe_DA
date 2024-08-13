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
    this.businessService.getUserNameFromStore().subscribe(res => {
      let userNameFromToken = this.businessService.getUserNameFromToken();
      this.userName = res || userNameFromToken;

      let params = new HttpParams().set('username', this.userName);
      
      this.appointmentService.GetAppointmentsByBusinessUserName(params).subscribe({
        next: (events: EventInput[]) => {
          this.eventList = events;
      
          // Get the calendar API
          const calendarApi = this.calendarComponent.getApi();
      
          // Remove all existing events
          calendarApi.removeAllEvents();
      
          // Add new events
          events.forEach(event => {
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
        customerName: ''
      }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result && this.calendarComponent) {
        const calendarApi = this.calendarComponent.getApi();
  
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
        
        const customerId = result.customerId; 
        console.log(customerId)

        const newAppointment: Appointment = {
          title: result.title,
          start: startDate,
          end: endDate,
          description: result.description,
          allDay: result.allDay
        };
        

        calendarApi.addEvent({
          title: newAppointment.title,
          start: newAppointment.start,
          end: newAppointment.end,
          extendedProps: {
            description: newAppointment.description
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

        const appointmentBody=JSON.stringify(newAppointment);

        console.log(appointmentBody)

        this.appointmentService.createAppointment(appointmentBody, params, headers).subscribe({
          next: response => console.log('Appointment saved:', response),
          error: err => console.error('Error saving appointment:', err)
        });
      } else {
        console.error('FullCalendarComponent instance is not initialized.');
      }
    });
  }

  //clicking on events
  handleEventClick(clickInfo: EventClickArg) {
    if (confirm(`Are you sure you want to delete the event '${clickInfo.event.title}'`)) {
      clickInfo.event.remove();
    }
  }

  //handling events
  handleEvents(events: EventApi[]) {
    this.currentEvents.set(events);
    this.cdr.detectChanges(); // workaround for pressionChangedAfterItHasBeenCheckedError
  }
}
