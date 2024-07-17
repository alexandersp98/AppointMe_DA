import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Customer } from '../classes/customer.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  url: string = environment.apiBaseUrl+'GetAllCustomers';
  list: Customer[] = [];
  
  constructor(private http: HttpClient) { }

  refreshList(){
    this.http.get(this.url)
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
