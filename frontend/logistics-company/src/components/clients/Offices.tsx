import { useEffect, useState } from "react";
import { OfficeModel } from "../../models/OfficeModel"
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { Avatar, Box, Button, Card, CardActions, CardContent, CardHeader, Grid, Modal, Typography } from "@mui/material";
import EmojiTransportationIcon from '@mui/icons-material/EmojiTransportation';
import Inventory2OutlinedIcon from '@mui/icons-material/Inventory2Outlined';
import PackageForm from "./PackageForm";
import CloseIcon from '@mui/icons-material/Close';

export default function Offices() {
    const API_URL = "https://localhost:7209/api";

    const style = {
        position: 'absolute' as 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: 400,
        bgcolor: 'background.paper',
        border: '2px solid #000',
        boxShadow: 24,
        pt: 2,
        px: 4,
        pb: 3,
    };

    const [open, setOpen] = useState<boolean>(false);
    const [offices, setOffices] = useState<OfficeModel[]>([]);
    const [users, setUsers] = useState<UserModel[]>([]);
    const [user, setUser] = useState<UserModel>(new UserModel(0, "Anonymous", ""));
    const jwt = sessionStorage["jwt"];

    useEffect(() => {
        if (jwt != null) {
            const { Id, Email, Role } = jwtDecode(jwt) as any;

            setUser((user: UserModel) => {
                user.id = Id;
                user.email = Email;
                user.role = Role;
                return user;
            })

            if (Role == "Client") {
                axios({
                    method: "GET",
                    url: `${API_URL}/Offices/GetAll`,
                    headers: {
                        "Authorization": `Bearer ${jwt}`
                    }
                })
                .then((response) => {
                    const data = response.data;

                    if (offices.length == 0 && data.length > 0) {
                        setOffices(data);
                    }
                })


                axios({
                    method: "GET",
                    url: `${API_URL}/Users/GetAllExcept?id=${user.id}`,
                    headers: {
                        "Authorization": `Bearer ${jwt}`
                    }
                })
                .then((response) => {
                    const data = response.data;
                    
                    if (users.length == 0 && data.length > 0) {
                        setUsers(data);
                    }
                })
            }

        }
    })
    console.log(users);
    
    function onClick() {
        setOpen(true);
    }
    function handleClose() {
        setOpen(false);
    }
    return (
        <>
            <Grid
                container
                spacing={2}
                marginTop="5%"
                sx={{
                    direction: "row",
                    justifyContent: "center",
                    alignItems: "center"
                }}
            >
                {offices.map(function (office) {
                    return (
                        <Grid item xs={12} sm={6} md={3} key={offices.indexOf(office)}>
                            <Card sx={{ maxWidth: 345 }}>
                                <CardHeader
                                    avatar={
                                        <Avatar aria-label="recipe">
                                            <EmojiTransportationIcon></EmojiTransportationIcon>
                                        </Avatar>
                                    }
                                >
                                </CardHeader>
                                <CardContent sx={{ marginLeft: "auto", marginRight: "auto" }}>
                                    <Typography gutterBottom variant="h5" component="div">
                                        {office.address}
                                    </Typography>
                                </CardContent>
                                <CardActions>
                                    <Button variant="contained"
                                        onClick={onClick}
                                        size="small"
                                        sx={{
                                            marginLeft: "auto",
                                            marginRight: "auto"
                                        }}
                                        color="primary">
                                        Submit Package
                                        <Inventory2OutlinedIcon sx={{
                                            marginLeft: "0.5%"
                                        }}>
                                        </Inventory2OutlinedIcon>
                                    </Button>
                                </CardActions>
                            </Card>
                        </Grid>
                    )
                })}
            </Grid>
            <Modal
                open={open}
                onClose={handleClose}
                aria-labelledby="parent-modal-title"
                aria-describedby="parent-modal-description"
            >
                <Box sx={{ ...style, width: 400 }}>
                    <CloseIcon onClick={handleClose} sx={{
                        float: "right",
                        cursor: "pointer"
                    }}>
                    </CloseIcon>
                    <PackageForm userId={user.id} users={users} />
                </Box>
            </Modal>
        </>
    )
}