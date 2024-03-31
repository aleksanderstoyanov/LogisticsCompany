export class LoginModel{
    email: string;
    passwordHash: string;

    constructor(email: string, password: string){
        this.email = email;
        this.passwordHash = password;
    }
}