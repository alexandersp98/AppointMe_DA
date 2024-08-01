import { Component, OnInit } from '@angular/core';
import { BusinessService } from '../../services/business.service';
import { CustomerService } from '../../services/customer.service';
import { CommonModule } from '@angular/common';
import { Business } from '../../classes/business.model';
import { HttpHeaders, HttpParams } from '@angular/common/http';
//import { log } from 'console';


@Component({
  selector: 'app-dev-site',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dev-site.component.html',
  styleUrl: './dev-site.component.scss'
})
export class DevSiteComponent implements OnInit {
  constructor(public businessService : BusinessService, public customerService : CustomerService){

  }


  ngOnInit(): void {
    this.businessService.refreshList();
    this.customerService.refreshList();


     //post test, FromBody
    let business = new Business('testtest3', 'test3@gmail.com', '1234567SDAFDaDD');

    const headers = new HttpHeaders()
    .append(
      'Content-Type',
      'application/json'
    );
    const body=JSON.stringify(business);
    this.businessService.postBusiness(body, headers);


    
    //login Check test, FromQuery
    let params = new HttpParams().set('username', 'MusterMaxi')
    .set('password', '123456Ab');

    /*this.businessService.LoginCheck(params);
    console.log('Post succeded');*/
  }

}
