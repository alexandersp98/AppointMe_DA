import { ChangeDetectorRef, Component, OnInit, signal, ViewChild } from '@angular/core';
import { FullCalendarComponent, FullCalendarModule } from '@fullcalendar/angular'; // Import FullCalendarComponent
import { CalendarOptions, EventApi, EventInput } from '@fullcalendar/core';
import { MatDialog } from '@angular/material/dialog';
import { EventDialogComponent } from './event-dialog/event-dialog.component';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';

import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { BusinessService } from '../../services/business.service';
import { AppointmentsService } from '../../services/appointments.service';
import { HttpParams } from '@angular/common/http';
import { CommonModule } from '@angular/common';


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

  @ViewChild(FullCalendarComponent) calendarComponent!: FullCalendarComponent;

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    dateClick: this.handleDateClick.bind(this),
    initialEvents: this.eventList,  // This will be updated dynamically
  };

  constructor(
    public dialog: MatDialog,
    public businessService: BusinessService,
    public appointmentService: AppointmentsService,
    private cdr: ChangeDetectorRef  // Inject ChangeDetectorRef
  ) {}

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
        description: ''
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

        calendarApi.addEvent({
          title: result.title,
          start: startDate,
          end: endDate,
          allDay: result.allDay,
          description: result.description
        });
        console.log('Event added:', {
          title: result.title,
          start: startDate,
          end: endDate,
          allDay: result.allDay,
          description: result.description
        });
      } else {
        console.error('FullCalendarComponent instance is not initialized.');
      }
    });
  }
}