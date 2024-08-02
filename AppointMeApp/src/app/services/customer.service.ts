import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Customer } from '../classes/customer.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  url: string = environment.apiBaseUrl+'GetCustomersByBusinessUserName';
  list: Customer[] = [];

  constructor(private http: HttpClient) { }

  refreshList(params: any){


    this.http.get(this.url, {params: params})
    .subscribe({
      next: res =>
        {
          this.list = res as Customer[];
          console.log(res);
        },
      error: err => {console.log(err)}
    })
  }
}
