import { Box, Container, Grid, TextField, Typography, Button } from "@mui/material";
import Person2OutlinedIcon from '@mui/icons-material/Person2Outlined';


import '../../styles/Register.css';
import "../../models/RegisterModel";

import { SyntheticEvent, useState } from "react";
import { RegisterModel } from "../../models/RegisterModel";
import axios, { Axios } from "axios";
import { API_URL, DEFAULT_USER_EMAIL, DEFAULT_USER_PASSWORD, DEFAULT_USER_USERNAME } from "../../util/Constants";

const IDS = ["user", "firstName", "lastName", "email", "password"] as const;
const [USERNAME_ID, FIRSTNAME_ID, LASTNAME_ID, EMAIL_ID, PASSWORD_ID] = IDS;

export function Register() {
  const [registerModel, changeRegisterModel] = useState<RegisterModel>
    (
        new RegisterModel
        (
          DEFAULT_USER_USERNAME,
          DEFAULT_USER_USERNAME,
          DEFAULT_USER_USERNAME,
          DEFAULT_USER_EMAIL,
          DEFAULT_USER_PASSWORD
        )
    );

  function onChange(event: SyntheticEvent) {
    let target = event.target as HTMLInputElement;
    let value = target.value;
    const id = target.getAttribute("id");

    switch (id) {
      case USERNAME_ID:
        changeRegisterModel((registerModel: RegisterModel) => {
          registerModel.username = value;
          return registerModel;
        })
        break;
      case FIRSTNAME_ID:
        changeRegisterModel((registerModel: RegisterModel) => {
          registerModel.firstName = value;
          return registerModel;
        })
        break;
      case LASTNAME_ID:
        changeRegisterModel((registerModel: RegisterModel) => {
          registerModel.lastName = value;
          return registerModel;
        })
        break;
      case EMAIL_ID:
        changeRegisterModel((registerModel: RegisterModel) => {
          registerModel.email = value;
          return registerModel;
        })
        break;
      case PASSWORD_ID:
        changeRegisterModel((registerModel: RegisterModel) => {
          registerModel.password = value;
          return registerModel;
        })
        break;
    }
  }

  function onCancel(event: SyntheticEvent) {
    IDS.forEach((id) => {
      let element = document.getElementById(id) as HTMLInputElement;
      element.value = "";
    })

    changeRegisterModel(new RegisterModel
      (
        DEFAULT_USER_USERNAME,
        DEFAULT_USER_USERNAME,
        DEFAULT_USER_USERNAME,
        DEFAULT_USER_EMAIL,
        DEFAULT_USER_PASSWORD
      )
    );
  }
  function onRegister(event: SyntheticEvent) {
    axios({
      method: 'POST',
      url: `${API_URL}/Authorization/Register`,
      data: registerModel
    })
      .then((response) => {
        if (response.status == 200) {
          window.location.href = "/login";
        }
      });
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
              Register
            </Typography>
            <Person2OutlinedIcon fontSize="large" />
          </Box>
          <TextField
            id={USERNAME_ID}
            fullWidth
            margin="normal"
            label="User Name"
            variant="outlined"
            onChange={onChange}
          />
          <TextField
            id={FIRSTNAME_ID}
            fullWidth
            margin="normal"
            label="First Name"
            variant="outlined"
            onChange={onChange}
          />
          <TextField
            id={LASTNAME_ID}
            fullWidth
            margin="normal"
            label="Last Name"
            variant="outlined"
            onChange={onChange}
          />
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
              <Button variant="outlined" color="primary" onClick={onRegister}>
                Register
              </Button>
            </Grid>
          </Grid>
        </Grid>
      </Grid>
    </Container>
  );
}