import { BaseUserModel } from "./BaseUserModel";

export class UserModel extends BaseUserModel{
    id: number;
    role: string;

    constructor(id: number, email: string, role: string) {
        super(email);
        this.id = id;
        this.role = role;
    }
}