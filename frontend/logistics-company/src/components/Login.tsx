import { Box, Button, Container, Grid, TextField, Typography } from "@mui/material";
import Person2OutlinedIcon from '@mui/icons-material/Person2Outlined';

import '../Register.css';
import "../models/RegisterModel";

import { SyntheticEvent, useState } from "react";
import { RegisterModel } from "../models/RegisterModel";
import { LoginModel } from "../models/LoginModel";


import axios, { Axios } from "axios";
import { jwtDecode } from "jwt-decode";

const API_URL = "https://localhost:7209";
const IDS = ["email", "password"] as const;
const [EMAIL_ID, PASSWORD_ID] = IDS;

export function Login() {
    const [loginModel, changeLoginModel] = useState<LoginModel>(new LoginModel("", ""));

    function onCancel(event: SyntheticEvent) {
        IDS.forEach((id) => {
            let element = document.getElementById(id) as HTMLInputElement;
            element.value = "";
        })
  
        changeLoginModel(new LoginModel("", ""));
    }
    function onLogin(event: SyntheticEvent) {
        axios({
          method: 'POST',
          url: `${API_URL}/api/Authorization/Login`,
          data: loginModel
        })
        .then((response) => {

            if(response != null){
                sessionStorage['jwt'] = response.data;
                console.log(jwtDecode<string>(response.data.toString()));
            }
        });
    }

    function onChange(event: SyntheticEvent) {
        let target = event.target as HTMLInputElement;
        let value = target.value;
        const id = target.getAttribute("id");

        switch (id) {
            case EMAIL_ID:
                changeLoginModel((loginModel: LoginModel) => {
                    loginModel.email = value;
                    return loginModel;
                })
                break;
            case PASSWORD_ID:
                changeLoginModel((loginModel: LoginModel) => {
                    loginModel.passwordHash = value;
                    return loginModel;
                })
                break;
        }
    }
    return (
        <Container maxWidth="sm">
            <Grid container justifyContent="center" alignItems="center" style={{ minHeight: "70vh" }}>
                <Grid>
                    <Box
                        display="flex"
                        alignItems="center"
                        justifyContent="center"
                        gap={2}>
                        <Typography variant="h4" component="h1" align="center">
                            Login
                        </Typography>
                        <Person2OutlinedIcon fontSize="large" />
                    </Box>
                    <TextField
                        id={EMAIL_ID}
                        fullWidth
                        margin="normal"
                        label="Email"
                        variant="outlined"
                        type="email"
                        onChange={onChange}
                    />
                    <TextField
                        id={PASSWORD_ID}
                        fullWidth
                        margin="normal"
                        label="Password"
                        type="password"
                        variant="outlined"
                        onChange={onChange}
                    />
                    <Grid container justifyContent="center" spacing={2} style={{ marginTop: 24 }}>
                        <Grid item>
                            <Button variant="outlined" onClick={onCancel}>
                                Cancel
                            </Button>
                        </Grid>
                        <Grid item>
                            <Button variant="outlined" color="primary" onClick={onLogin}>
                                Login
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Container>
    )
}