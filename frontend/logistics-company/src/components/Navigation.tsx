import { AppBar, Box, Button, Icon, IconButton, Link, Toolbar, Typography } from "@mui/material";
import { jwtDecode } from "jwt-decode";
import { useEffect, useState } from "react";
import { UserModel } from "../models/UserModel";

export function Navigation() {

    const [userModel, setUserModel] = useState<UserModel>(new UserModel("Anonymous", "None"));

    const [isRegisterVisible, setRegisterVisible] = useState<boolean>(true);
    const [isAdminPanelVisible, setAdminPanelVisible] = useState<boolean>(false);
    const [isLoginVisible, setLoginVisible] = useState<boolean>(true);
    const [isLogoutVisible, setLogoutVisible] = useState<boolean>(false);

    const jwt = sessionStorage["jwt"];

    useEffect(() => {
        if (jwt != null) {
            setRegisterVisible(false);
            setLoginVisible(false);
            setLogoutVisible(true);


            const { Email, Role } = jwtDecode(jwt) as any;

            if (Role == "Admin") {
                setAdminPanelVisible(true);
            }
            setUserModel((userModel: UserModel) => {

                userModel.email = Email;
                userModel.role = Role;
                return userModel;
            })

            console.log(userModel);
        }
    });

    function onLogout() {
        sessionStorage.removeItem("jwt");
        setLoginVisible(true);
        setRegisterVisible(true);
        setLogoutVisible(false);
    }

    function isVisible(visible: boolean) {
        return `${visible ? "inline" : "none"}`;
    }

    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static">
                <Toolbar>
                    <Typography variant="h6" sx={{ flexGrow: 1 }} style={{ marginTop: "5" }}>
                        <Link underline="none" href="/">
                            Logistics Company
                        </Link>
                        <Link variant="h6" underline="none" href="/adminPanel" style={{ display: isVisible(isAdminPanelVisible), marginLeft: "3%"  }}>
                            <Button color="inherit">
                                Admin Panel
                            </Button>
                        </Link>
                        <Link variant="h6" underline="none" href="/officePanel" style={{ display: isVisible(isAdminPanelVisible), marginLeft: "2%"  }}>
                            <Button color="inherit">
                                Office Panel
                            </Button>
                        </Link>
                    </Typography>
                    <Typography variant="h6" component="div">
                        <Link variant="h6" href="login" underline="none" style={{ display: isVisible(isLoginVisible) }}>
                            <Button color="inherit">
                                Login
                            </Button>
                        </Link>
                        <Link variant="h6" href="register" underline="none" style={{ display: isVisible(isRegisterVisible) }} >
                            <Button color="inherit">
                                Register
                            </Button>
                        </Link>
                        <Link variant="h6" href="/" underline="none" style={{ display: isVisible(isLogoutVisible) }} >
                            <Button color="inherit" onClick={onLogout}>
                                Logout
                            </Button>
                        </Link>
                    </Typography>
                </Toolbar>
            </AppBar>
        </Box>
    );
}