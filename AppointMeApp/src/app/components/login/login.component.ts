import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BusinessService } from '../../services/business.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Business } from '../../classes/business.model';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm = new FormGroup({
    userName: new FormControl('', Validators.required),
    password: new FormControl('', [Validators.required, Validators.minLength(6)])
  });

  constructor(private businessService: BusinessService, private snackBar: MatSnackBar, private router: Router){}

  submitLogin(){
    if (this.loginForm.valid) 
      {
      const business = new Business(
        this.loginForm.value.userName ?? '',
        '',
        this.loginForm.value.password ?? ''
      );

      const headers = new HttpHeaders()
      .append(
        'Content-Type',
        'application/json'
      );
      const body=JSON.stringify(business);

      this.businessService.login(body, headers).subscribe({
        next: (res) => {
          console.log(res.message)
          this.businessService.storeToken(res.token);
          const tokenPayload = this.businessService.decodedToken();
          this.businessService.setUserNameForStore(tokenPayload.unique_name);
          this.router.navigate(['dashboard']);
        },
        error: (err) => {
          console.log(err.error.message)
          
          this.snackBar.open('The entered Login data is invalid.', 'Close', {
            duration: 3000,
          });

          this.loginForm.reset();
        }
      })
     }     
    }

}
