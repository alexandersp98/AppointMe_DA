import { Component } from '@angular/core';
import { Business } from '../../classes/business.model';
import { FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';
import { BusinessService } from '../../services/business.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';


@Component({
  selector: 'app-create-account',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.scss'
})

export class CreateAccountComponent
{
  registerForm = new FormGroup({
    userName: new FormControl('', Validators.required),
    e_Mail_Address: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)])
  });

  constructor(private businessService: BusinessService, private snackBar: MatSnackBar, private router: Router){}

  submitRegistration() 
  {
  if (this.registerForm.valid) 
    {
    const business = new Business(
      this.registerForm.value.userName ?? '',
      this.registerForm.value.e_Mail_Address ?? '',
      this.registerForm.value.password ?? ''
    );

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    this.businessService.postBusiness(business, headers).subscribe({
      next: (response: any) => {
        this.snackBar.open('Registration successful!', 'Close', {
          duration: 3000,
        });

        this.router.navigate(['/login']);
      },
      error: (error: any) => {
        this.snackBar.open('Registration failed. Please try again.', 'Close', {
          duration: 3000,
        });

        this.registerForm.reset();
      }
    });
   }
   else{
    this.snackBar.open('Your entered credentials don\'t match our savety requirements\n please make sure that your password is at least 6 characters long and contains one capital letter', 'Close', {
      duration: 10000,
    });
   }
  }
}


