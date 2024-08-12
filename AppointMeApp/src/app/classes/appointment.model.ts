import { Customer } from './customer.model';

export class Appointment {

    title: string = "";
    start: Date = new Date();
    end: Date = new Date();
    description: string = "";
    allDay: boolean = false;

    constructor(
        Title: string,
        AllDay: boolean,
        Start: Date,
        End: Date,
        Description: string,
    ) {
        this.title = Title;
        this.allDay = AllDay;
        this.start = Start;
        this.end = End;
        this.description = Description;
    }
}
