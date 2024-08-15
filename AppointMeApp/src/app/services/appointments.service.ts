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
  urlDeleteAppointment: string = environment.apiBaseUrl + 'Appointment';

  list: Appointment[] = [];

  refreshList(params: any){
    this.http.get(this.urlAllAppointmentsByBusinessUserName, {params: params})
    .subscribe({
      next: res =>
        {
          this.list = res as Appointment[];
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

          for (const appointment of appointments) {
            const event: EventInput = {
              id: String(appointment.id),
              title: appointment.title,
              start: appointment.start,
              end: appointment.end,
              extendedProps: {
                description: appointment.extendedProps?.description ?? 'No description',
                customerId: appointment.extendedProps?.customerId ?? 0
              }
            };
            console.log(event);
            eventInputs.push(event);
          }

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
    console.log('Creating appointment with body:', appointment); // Log the body to debug
    return this.http.post<any>(this.urlCreateAppointment, appointment, { params, headers, responseType: 'text' as 'json' });
  }

  deleteAppointment(appointmentId: number): Observable<void> {
    const params = new HttpParams().set('appointmentId', appointmentId.toString());
    return this.http.delete<void>(this.urlDeleteAppointment, { params });
  }
}
