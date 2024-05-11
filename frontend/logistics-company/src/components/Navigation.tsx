import { AppBar, Box, Button, Link, Toolbar, Typography } from "@mui/material";
import { jwtDecode } from "jwt-decode";
import { useEffect, useState } from "react";
import { UserModel } from "../models/UserModel";

export function Navigation() {

    const [userModel, setUserModel] = useState<UserModel>(new UserModel(0, "Anonymous", "None"));

    const [isOfficesVisible, setOfficesVisible] = useState<boolean>(false);
    const [isDeliveriesVisible, setDeliveriesVisible] = useState<boolean>(false);

    const [isPackagesVisible, setPackagesVisible] = useState<boolean>(false);
    const [isSentPackagesVisible, setSentPackagesVisible] = useState<boolean>(false);
    const [isReceivedPackagesVisible, setReceivedPackagesVisible] = useState<boolean>(false);

    const [isAdminPanelVisible, setAdminPanelVisible] = useState<boolean>(false);

    const [isRegisterVisible, setRegisterVisible] = useState<boolean>(true);
    const [isLoginVisible, setLoginVisible] = useState<boolean>(true);
    const [isLogoutVisible, setLogoutVisible] = useState<boolean>(false);

    const jwt = sessionStorage["jwt"];

    useEffect(() => {
        if (jwt != null) {
            setRegisterVisible(false);
            setLoginVisible(false);
            setLogoutVisible(true);


            const { Email, Role } = jwtDecode(jwt) as any;

            switch (Role) {
                case "Admin":
                    setAdminPanelVisible(true);
                    break;
                case "Client":
                    setOfficesVisible(true);
                    setSentPackagesVisible(true);
                    setReceivedPackagesVisible(true);
                    break;
                case "OfficeEmployee":
                    setPackagesVisible(true);
                    break;
                case "Courier":
                    setPackagesVisible(true);
                    setDeliveriesVisible(true);
                    break;
            }

            setUserModel((userModel: UserModel) => {
                userModel.email = Email;
                userModel.role = Role;
                return userModel;
            })
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
                        <Link variant="h6" underline="none" href="/adminPanel" style={{ display: isVisible(isAdminPanelVisible), marginLeft: "3%" }}>
                            <Button color="inherit">
                                Admin Panel
                            </Button>
                        </Link>
                        <Link variant="h6" underline="none" href="/officePanel" style={{ display: isVisible(isAdminPanelVisible), marginLeft: "2%" }}>
                            <Button color="inherit">
                                Office Panel
                            </Button>
                        </Link>
                        <Link variant="h6" href="reports" underline="none" style={{ display: isVisible(isAdminPanelVisible), marginLeft: "2%" }}>
                            <Button color="inherit">
                                Reports
                            </Button>
                        </Link>
                        <Link variant="h6" underline="none" href="/offices" style={{ display: isVisible(isOfficesVisible), marginLeft: "2%" }}>
                            <Button color="inherit">
                                Offices
                            </Button>
                        </Link>
                        <Link variant="h6" underline="none" href="/deliveries" style={{ display: isVisible(isDeliveriesVisible), marginLeft: "2%" }}>
                            <Button color="inherit">
                                Deliveries
                            </Button>
                        </Link>
                        <Link variant="h6" underline="none" href="/packages" style={{ display: isVisible(isPackagesVisible), marginLeft: "2%" }}>
                            <Button color="inherit">
                                Packages
                            </Button>
                        </Link>
                        <Link variant="h6" underline="none" href="/receivedPackages" style={{ display: isVisible(isReceivedPackagesVisible), marginLeft: "2%" }}>
                            <Button color="inherit">
                                Received Packages
                            </Button>
                        </Link>
                        <Link variant="h6" underline="none" href="/sentPackages" style={{ display: isVisible(isSentPackagesVisible), marginLeft: "2%" }}>
                            <Button color="inherit">
                                Sent Packages
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