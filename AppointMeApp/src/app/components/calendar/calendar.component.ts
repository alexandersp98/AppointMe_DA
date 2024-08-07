import { Component, ViewChild } from '@angular/core';
import { FullCalendarComponent, FullCalendarModule } from '@fullcalendar/angular'; // Import FullCalendarComponent
import { CalendarOptions } from '@fullcalendar/core';
import { MatDialog } from '@angular/material/dialog';
import { EventDialogComponent } from './event-dialog/event-dialog.component';

import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-calendar',
  standalone: true,
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
  imports: [
    CommonModule,
    FullCalendarModule, // Import FullCalendarModule here
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    EventDialogComponent
  ]
})
export class CalendarComponent {
  @ViewChild(FullCalendarComponent) calendarComponent!: FullCalendarComponent;

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    dateClick: this.handleDateClick.bind(this),
    events: [
      { title: 'event 1', date: '2024-08-06' },
      { title: 'event 2', start: '2024-08-08T16:00:00', end: '2024-08-09T13:00:00' },
      { title: 'event 3', date: '2024-08-06T13:00:00' },
      { title: 'event 4', date: '2024-08-06T09:00:00', url: 'https://github.com/' }
    ]
  };

  constructor(public dialog: MatDialog) { }

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
        const calendarApi = this.calendarComponent.getApi(); // Use FullCalendar API

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
