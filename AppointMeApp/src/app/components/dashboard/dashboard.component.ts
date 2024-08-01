import { Component, OnInit } from '@angular/core';
import { BusinessService } from '../../services/business.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})


export class DashboardComponent implements OnInit{
  constructor(public businessService: BusinessService){}

  logOut(){
   this.businessService.signOut();
  }

  ngOnInit(): void {
    this.businessService.refreshList();
  }

}
