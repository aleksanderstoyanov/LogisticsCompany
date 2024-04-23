import { BaseUserModel } from "./BaseUserModel";

export class RegisterModel extends BaseUserModel {
    username: string;
    password: string;
    firstName: string;
    lastName: string;

    constructor(username: string, firstName: string, lastName: string, email: string, password: string){
        super(email);
        this.username = username;
        this.password = password;
        this.firstName = firstName;
        this.lastName = lastName;
    }
}