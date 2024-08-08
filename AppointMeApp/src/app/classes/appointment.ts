export class Appointment {
    id: number = 0;
    title: string = "";
    allDay: boolean = false;
    start: Date = new Date;
    end: Date = new Date;
    description: string = "";

    constructor(
        Title: string,
        AllDay: boolean,
        Start: Date,
        End: Date,
        Description: string) 
        {
            this.title  = Title;
            this.allDay = AllDay;
            this.start = Start;
            this.end = End;
            this.description = Description;
        }
}
