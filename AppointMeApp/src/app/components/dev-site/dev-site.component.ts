import { Component, OnInit } from '@angular/core';
import { BusinessService } from '../../services/business.service';
import { CustomerService } from '../../services/customer.service';
import { CommonModule } from '@angular/common';


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
  }

}
