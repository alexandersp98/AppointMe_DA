import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventInput } from '@fullcalendar/core';
import { environment } from '../../environments/environment.development';
import { Appointment } from '../classes/appointment.model';

@Injectable({
  providedIn: 'root'
})
export class AppointmentsService {

  constructor(private http: HttpClient) {}

  urlAllAppointmentsByBusinessUserName: string = environment.apiBaseUrl + 'GetAppointmentsByBusinessUserName';
  urlCreateAppointment: string = environment.apiBaseUrl + 'CreateAppointment';
  list: Appointment[] = [];

  refreshList(params: any){
    this.http.get(this.urlAllAppointmentsByBusinessUserName, {params: params})
    .subscribe({
      next: res =>
        {
          this.list = res as Appointment[];
          console.log(res);
        },
      error: err => {console.log(err)}
    })
  }

  GetAppointmentsByBusinessUserName(params: HttpParams): Observable<EventInput[]> {
    return new Observable<EventInput[]>((observer) => {
      this.http.get<Appointment[]>(this.urlAllAppointmentsByBusinessUserName, { params })
      .subscribe({
        next: res => {
          const appointments = res as Appointment[];
          const eventInputs: EventInput[] = [];

          console.log('Appointments received:', appointments.length); // Will log the correct length

          for (let i = 0; i < appointments.length; i++) {
            const event: EventInput = {
              title: appointments[i].title,
              start: appointments[i].start,  
              end: appointments[i].end
            };
            eventInputs.push(event);
            
          }

          console.log('EventInputs created:', eventInputs.length); // Will log the created EventInputs length
          observer.next(eventInputs);
          observer.complete();
        },
        error: err => {
          console.log(err);
          observer.error(err);
        }
      });
    });
  }

  createAppointment(appointment: any, params: HttpParams, headers: HttpHeaders): Observable<any> {
    return this.http.post<any>(this.urlCreateAppointment, appointment, { params, headers, responseType: 'text' as 'json' });
  }
}
