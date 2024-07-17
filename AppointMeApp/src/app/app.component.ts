import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./components/login/login.component";
import { DevSiteComponent } from './components/dev-site/dev-site.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { FormularComponent } from './components/formular/formular.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginComponent, DevSiteComponent, CalendarComponent, FormularComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})


export class AppComponent {
  title = 'AppointMe';
  
}
