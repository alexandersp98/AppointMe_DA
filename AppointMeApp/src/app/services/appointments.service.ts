import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DateInput, EventInput } from '@fullcalendar/core';
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
  urlUpdateAppointment: string = environment.apiBaseUrl + 'UpdateAppointment'; 

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
              allDay: appointment.allDay,
              start: this.convertUtcToLocal(appointment.start),
              end: this.convertUtcToLocal(appointment.end),
              extendedProps: {
                description: appointment.extendedProps?.description ?? 'No description',
                customerId: appointment.extendedProps?.customerId ?? 0
              }
            };
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
  
  convertUtcToLocal(utcDate: DateInput | undefined): Date | undefined {
    if (!utcDate) {
      return undefined;
    }
  
    let date: Date;
  
    if (utcDate instanceof Date) {
      date = utcDate;
    } else if (typeof utcDate === 'string' || typeof utcDate === 'number') {
      date = new Date(utcDate);
    } else if (Array.isArray(utcDate)) {
      // If utcDate is a number array, treat it as [year, month, day, hour, minute, second]
      const [year, month, day, hour = 0, minute = 0, second = 0] = utcDate;
      date = new Date(year, month - 1, day, hour, minute, second);
    } else {
      throw new Error('Invalid date format');
    }
  
    return new Date(date.getTime() - date.getTimezoneOffset() * 60000);
  }
  
  createAppointment(appointment: any, params: HttpParams, headers: HttpHeaders): Observable<any> {
    return this.http.post<any>(this.urlCreateAppointment, appointment, { params, headers, responseType: 'text' as 'json' });
  }

  deleteAppointment(appointmentId: number): Observable<void> {
    const params = new HttpParams().set('appointmentId', appointmentId.toString());
    return this.http.delete<void>(this.urlDeleteAppointment, { params });
  }

  updateAppointment(appointment: any, params: HttpParams, headers: HttpHeaders): Observable<any> {
    return this.http.put<any>(this.urlUpdateAppointment, appointment, { params, headers });
  }
  

}
