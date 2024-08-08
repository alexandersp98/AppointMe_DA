
export class Business {
    id: number =  0;
    userName: string = "";
    e_Mail_Address: string = "";
    password: string = "";

    constructor(
        UserName: string,
        E_Mail_Address: string,
        Password: string) 
        
        {
            this.userName  = UserName;
            this.e_Mail_Address = E_Mail_Address;
            this.password = Password;
        }
}
