import { Component } from '@angular/core';
import { TestService } from './test.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent {
  title = 'AppointMeApp';


  constructor(public service: TestService){


    service.GetHello();



  }



}
