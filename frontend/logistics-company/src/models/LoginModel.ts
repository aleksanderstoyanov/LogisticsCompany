import { BaseUserModel } from "./BaseUserModel";

export class LoginModel extends BaseUserModel{
    passwordHash: string;

    constructor(email: string, password: string){
        super(email);
        this.passwordHash = password;
    }
}