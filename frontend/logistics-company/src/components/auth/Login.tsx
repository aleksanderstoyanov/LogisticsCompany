import { Alert, Box, Button, Container, Grid, TextField, Typography } from "@mui/material";
import Person2OutlinedIcon from '@mui/icons-material/Person2Outlined';

import "../../models/RegisterModel";
import '../../styles/Register.css';

import { SyntheticEvent, useState } from "react";
import { LoginModel } from "../../models/LoginModel";

import axios from "axios";
import { API_URL, DEFAULT_USER_PASSWORD, DEFAULT_USER_USERNAME, LOGIN_FORM_IDS } from "../../util/Constants";

export function Login() {
    const [EMAIL_ID, PASSWORD_ID] = LOGIN_FORM_IDS;
    const [loginModel, setLoginModel] = useState<LoginModel>
    (
        new LoginModel
        (
             DEFAULT_USER_USERNAME,
             DEFAULT_USER_PASSWORD
        )
    );
    const [isErrorVisible, setErrorVisiblitiy] = useState<boolean>(false);

    function onCancel(event: SyntheticEvent) {
        LOGIN_FORM_IDS.forEach((id) => {
            let element = document.getElementById(id) as HTMLInputElement;
            element.value = "";
        })

        setLoginModel(new LoginModel("", ""));
    }
    function onLogin(event: SyntheticEvent) {
        axios({
            method: 'POST',
            url: `${API_URL}/Authorization/Login`,
            data: loginModel
        })
            .then((response) => {
                debugger;
                let jwt = response.data;
                if (jwt != "Invalid Credentials") {
                    sessionStorage['jwt'] = response.data;
                    if (isErrorVisible) {
                        setErrorVisiblitiy(false);
                    }
                    window.location.href = "/";
                }
            })
            .catch(() => {
                setErrorVisiblitiy(true);

                setLoginModel(new LoginModel("", ""));
                let cancelBtn = document.getElementById("cancelBtn") as HTMLButtonElement;
                cancelBtn.click();

                setTimeout(async function(){
                    await setErrorVisiblitiy(false);
                }, 1000)
            });
    }

    function onChange(event: SyntheticEvent) {
        let target = event.target as HTMLInputElement;
        let value = target.value;
        const id = target.getAttribute("id");

        switch (id) {
            case EMAIL_ID:
                setLoginModel((loginModel: LoginModel) => {
                    loginModel.email = value;
                    return loginModel;
                })
                break;
            case PASSWORD_ID:
                setLoginModel((loginModel: LoginModel) => {
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
                            <Button id="cancelBtn" variant="outlined" onClick={onCancel}>
                                Cancel
                            </Button>
                        </Grid>
                        <Grid item>
                            <Button id="loginBtn" variant="outlined" color="primary" onClick={onLogin}>
                                Login
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
            <Alert style={{ display: isErrorVisible ? "block" : "none" }} severity="error">
                Credentials are invalid.
            </Alert>
        </Container>
    )
}