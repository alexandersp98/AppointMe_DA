export class Appointment {
    id: any;
    title: string;
    allDay: boolean;
    start: Date;
    end: Date;
    extendedProps?: { // Make sure this matches the structure
        description?: string;
        customerId?: number;
      };

    constructor(
        Id: any,
        Title: string,
        AllDay: boolean,
        Start: Date,
        End: Date,
        extendedProps?: { description?: string, customerId?: number }
    ) {
        this.id = Id;
        this.title = Title;
        this.allDay = AllDay;
        this.start = Start;
        this.end = End;
        this.extendedProps = extendedProps;
    }
}