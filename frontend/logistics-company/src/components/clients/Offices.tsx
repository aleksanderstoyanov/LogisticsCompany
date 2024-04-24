import { useEffect, useState } from "react";
import { OfficeModel } from "../../models/OfficeModel"
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { Avatar, Button, Card, CardActions, CardContent, CardHeader, Grid, Typography } from "@mui/material";
import EmojiTransportationIcon from '@mui/icons-material/EmojiTransportation';
import Inventory2OutlinedIcon from '@mui/icons-material/Inventory2Outlined';

export default function Offices() {
    const API_URL = "https://localhost:7209/api";
    const [offices, setOffices] = useState<OfficeModel[]>([]);
    const [user, setUser] = useState<UserModel>(new UserModel(0, "Anonymous", ""));
    const jwt = sessionStorage["jwt"];

    useEffect(() => {
        if (jwt != null) {
            const { Email, Role } = jwtDecode(jwt) as any;

            setUser((user: UserModel) => {
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
            }

        }
    })

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
        </>
    )
}