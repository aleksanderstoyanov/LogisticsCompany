import { BaseUserModel } from "./BaseUserModel";

export class UserModel extends BaseUserModel{
    role: string;

    constructor(email: string, role: string) {
        super(email);
        this.role = role;
    }
}