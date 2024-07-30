import { Component, OnInit } from '@angular/core';
import { Business } from '../../classes/business.model';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-account',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.scss'
})
export class CreateAccountComponent implements OnInit{
  businessForm: FormGroup;

  constructor(private fb: FormBuilder) {

    this.businessForm = this.fb.group({
      userName: ['', Validators.required],
      e_Mail_Address: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }
  ngOnInit(): void {
    console.log('Method not implemented.');
  }


  onSubmit() {
    if(this.businessForm.valid) {
      const business = new Business(
        this.businessForm.value.userName,
        this.businessForm.value.e_Mail_Address,
        this.businessForm.value.password
      );

      console.log('Business: ', business);
    }

    /*const headers = new HttpHeaders()
    .append(
      'Content-Type',
      'application/json'
    );

    const body=JSON.stringify(business);



    this.businessService.postBusiness(body, headers);*/
  }

}
