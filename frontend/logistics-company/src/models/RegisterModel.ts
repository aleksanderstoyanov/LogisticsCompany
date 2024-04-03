import { BaseUserModel } from "./BaseUserModel";

export class RegisterModel extends BaseUserModel {
    username: string;
    password: string;

    constructor(username: string, email: string, password: string){
        super(email);
        this.username = username;
        this.password = password;
    }
}