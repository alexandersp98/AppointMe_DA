import { Component } from '@angular/core';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CommonModule } from '@angular/common';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';


@Component({
  selector: 'app-calendar',
  standalone: true,
  imports: [
    FullCalendarModule,
    CommonModule
  ],
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent {
  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    dateClick: (arg: any) => this.handleDateClick(arg),
    events: [
      { title: 'event 1', date: '2024-08-06' },
      { title: 'event 2', date: '2024-08-08' }
    ],
    eventContent: (arg: any) => {
      return {
        html: `<div class="custom-event">${arg.event.title}</div>`
      };
    }
  };

  handleDateClick(date: any) {
    alert('date click! ' + date.dateStr);
  }
}
