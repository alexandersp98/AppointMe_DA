import { Business } from './../classes/business.model';
import { Injectable, SkipSelf } from '@angular/core';
import { HttpClient, HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpParams, HttpRequest } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';


@Injectable({
  providedIn: 'root'
})

export class BusinessService{

  urlAllBusiness: string = environment.apiBaseUrl+'GetAllBusinesses';
  urlPost: string = environment.apiBaseUrl + 'Business';
  urlLoginCheck: string = environment.apiBaseUrl + 'BusinessLoginCheck';
  urlLogin: string = environment.apiBaseUrl + 'Authenticate';
  list: Business[] = [];
  userNamePayload: any; 
  
  private userName$ = new BehaviorSubject<string>("")
  constructor(private http: HttpClient, private router: Router) {
    this.userNamePayload = this.decodedToken();
   }

  refreshList(){
    this.http.get(this.urlAllBusiness)
    .subscribe({
      next: res =>
        {
          this.list = res as Business[];
        },
      error: err => {console.log(err)}
    })
  }

  //sign up
  postBusiness(newBusiness: any, headers: any): Observable<any>
  {
    return this.http.post<any>(this.urlPost, newBusiness, {headers : headers});
  }

  //sign in
  login(loginObj: any, headers: any){
    return this.http.post<any>(this.urlLogin, loginObj, {headers : headers})
  }

  /*LoginCheck(items: HttpParams)
  {
    this.http.get(this.urlLoginCheck, {params: items}).subscribe(

      res => {console.log(res);}

    )
  }*/

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue)
  }

  getToken(){
    return localStorage.getItem('token')
  }

  isLoggedIn():boolean{
    return !!localStorage.getItem('token')
  }

  signOut(){
    localStorage.clear();
    this.router.navigate(['login']);
  }

  decodedToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    console.log(jwtHelper.decodeToken(token));
    return jwtHelper.decodeToken(token);
  }

  getUserNameFromToken(){
    if(this.userNamePayload)
    {
      return this.userNamePayload.unique_name;
    }
  }

  public getUserNameFromStore(){
    return this.userName$.asObservable();
  }

  public setUserNameForStore(userName:string){
    this.userName$.next(userName);
  }


}
