import { Business } from "./business.model";
import { Customer } from "./customer.model";

export class Chat {
    id: number = 0;
    customer: Customer = new Customer; 
    business: Business = new Business("", "", ""); 
}
