import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { BusinessService } from '../services/business.service';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';


export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const businessService = inject(BusinessService);
  const snackBar = inject(MatSnackBar)
  const router = inject(Router)

  if(businessService.isLoggedIn())
  {
    console.log('zugelassen');
    return true;
  }
  else{
    console.log('abgewiesen');
    snackBar.open('Please Login first!', 'Close', {
      duration: 3000,
    });
    router.navigate(['login']);
    return false;
  }

  
};


