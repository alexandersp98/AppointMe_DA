import { Routes } from '@angular/router';
import { DevSiteComponent } from './components/dev-site/dev-site.component';
import { LoginComponent } from './components/login/login.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { FormularComponent } from './components/formular/formular.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { CreateAccountComponent } from './components/create-account/create-account.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

export const routes: Routes = [
    {
        path: '',
        component: DevSiteComponent
    },

    {
        path: 'devSite',
        component: DevSiteComponent
    },

    {
        path: 'login',
        component: LoginComponent
    },

    {
        path: 'calendar',
        component: CalendarComponent
    },

    {
        path: 'formular',
        component: FormularComponent
    },

    {
        path: 'resetPassword',
        component: ResetPasswordComponent
    },

    {
        path: 'createAccount',
        component: CreateAccountComponent
    },

    {
        path: 'dashboard',
        component: DashboardComponent
    },

];
