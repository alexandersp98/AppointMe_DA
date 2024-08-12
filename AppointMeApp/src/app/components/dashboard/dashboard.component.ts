import { Component, OnInit } from '@angular/core';
import { BusinessService } from '../../services/business.service';
import { CommonModule } from '@angular/common';
import { HttpParams } from '@angular/common/http';
import { CustomerService } from '../../services/customer.service';
import { AppointmentsService } from '../../services/appointments.service';
import { ChatService } from '../../services/chat.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})


export class DashboardComponent implements OnInit{
  constructor(public businessService: BusinessService, public appointmentService: AppointmentsService, public chatService: ChatService){}

  public userName: string = "";

  logOut(){
   this.businessService.signOut();
  }

  ngOnInit(): void {

    this.businessService.getUserNameFromStore().subscribe(res => {
      let userNameFromToken = this.businessService.getUserNameFromToken();
      this.userName = res ||userNameFromToken;
    })

    let params = new HttpParams().set('username', this.userName);

    this.appointmentService.refreshList(params);
    this.chatService.getAllChats();
  }

}
