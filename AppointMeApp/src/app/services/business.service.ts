import { Business } from './../classes/business.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
//import { error } from 'console';

@Injectable({
  providedIn: 'root'
})

export class BusinessService {

  urlAllBusiness: string = environment.apiBaseUrl+'GetAllBusinesses';
  urlPost: string = environment.apiBaseUrl + 'Business';
  urlLoginCheck: string = environment.apiBaseUrl + 'BusinessLoginCheck';
  list: Business[] = [];

  constructor(private http: HttpClient) { }

  refreshList(){
    this.http.get(this.urlAllBusiness)
    .subscribe({
      next: res =>
        {
          this.list = res as Business[];
          console.log(res);
        },
      error: err => {console.log(err)}
    })
  }

  postBusiness(newBusiness: any, headers: any){
    this.http.post(this.urlPost, newBusiness,

 {headers : headers}



    ).subscribe( res => {console.log(res)});

  }

  LoginCheck(items: HttpParams)
  {

    this.http.get(this.urlLoginCheck, {params: items})
    .subscribe(

      res => {console.log(res);}

    )
  }



}
