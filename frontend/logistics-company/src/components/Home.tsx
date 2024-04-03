import { Typography } from "@mui/material";
import { UserModel } from "../models/UserModel";
import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

export function Home() {
    const jwt = sessionStorage["jwt"];
    const [userModel, setUserModel] = useState<UserModel>(new UserModel("Anonymous", "None"));

    let message = `Welcome ${userModel.email}, to Logistics Company!`;
    
    useEffect(() => {
        if (jwt != null) {
            const { Email, Role } = jwtDecode(jwt.toString()) as any;
            setUserModel((userModel: UserModel) => {
                userModel.email = Email;
                userModel.role = Role;
                return userModel;
            })

            let title = document.getElementById("title") as HTMLElement;
            message = `Welcome ${userModel.email}, to Logistics Company!`;

            title.textContent = message;
        }
    })


    return (
        <Typography id="title" variant="h4" component="h1" align="center">
            Welcome {userModel.email}, to Logistics Company!
        </Typography>
    )
}