import { useEffect, useState } from "react";
import { UserModel } from "../../../models/UserModel";
import { Box, Grid, InputLabel, MenuItem, Select, SelectChangeEvent, Typography } from "@mui/material";
import PackageReportDetails from "./PackageReportDetails";
import { PackageStatusReportModel } from "../../../models/PackageStatusReportModel";
import { getReceivedPackages, getSentPackages } from "../../../requests/PackageRequests";
import { getAllClients } from "../../../requests/ReportRequests";

export default function UserPackageDetails(props: any) {
    const jwt = sessionStorage["jwt"];

    const [clients, setClients] = useState<UserModel[]>([]);
    const [packages, setPackages] = useState<PackageStatusReportModel[]>([]);

    useEffect(() => {
        getAllClients(jwt)
            .then((response) => {
                const data = response.data.data;
                if (clients.length == 0 && data.length > 0) {
                    setClients(data);
                }
            })
    }, []);

    function onChange(event: SelectChangeEvent) {
        const userId = event.target.value;
        let response;

        switch (props.detailsFor) {
            case "ReceivedPackages":
                response = getReceivedPackages(jwt, userId);
                break;
            case "SentPackages":
                response = getSentPackages(jwt, userId);
                break;
        }

        response = props.detailsFor == "ReceivedPackages" ? getReceivedPackages(jwt, userId) : getSentPackages(jwt, userId);

        response.then((response) => {
            const data = response.data;
            setPackages(data);
        })
    }

    return (
        <Box>
            <Box sx={{
                display: "flex",
                justifyContent: "center",
                alignContent: "center"
            }}>
                <Typography variant="h6">
                    {props.detailsFor == "ReceivedPackages" ? "Received Packages For" : "Sent Packages By"}
                </Typography>
            </Box>
            <Grid sx={{
                marginTop: "5em"
            }}>
                <InputLabel id="clients">
                    Clients
                </InputLabel>
                <Select id="clients"
                    defaultValue={"0"}
                    fullWidth={true}
                    onChange={onChange}>
                    {clients.map((client) => {
                        return (
                            <MenuItem key={client.id.toString()} value={client.id}>
                                {client.email}
                            </MenuItem>
                        )
                    })}
                </Select>
            </Grid>
            <PackageReportDetails detailsFor={props.detailsFor} packages={packages}></PackageReportDetails>
        </Box>
    );
}