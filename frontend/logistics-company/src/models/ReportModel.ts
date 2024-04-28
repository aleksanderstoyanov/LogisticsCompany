export class ReportUserModel{
    firstName: string;
    lastName: string;
    roleName: string;
    officeName: string

    constructor(firstName: string, lastName: string, roleName: string, officeName: string){
        this.firstName = firstName;
        this.lastName = lastName;
        this.roleName = roleName;
        this.officeName = officeName;
    }
}