import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Test } from './test';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  url: string = 'https://localhost:7161/Home'





  constructor(private http: HttpClient) { }


  GetHello(){

    this.http.get(this.url).subscribe({next: res=> {

      console.log(res);



    },
    error: err => {console.log(err);}


  })

}


}
