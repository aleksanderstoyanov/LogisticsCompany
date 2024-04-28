import { Box, Link, Modal, Typography } from "@mui/material";
import { API_URL, MODAL_STYLE } from "../../util/Constants";
import { Component, SyntheticEvent, useEffect, useState } from "react";
import axios from "axios";
import { isAuthorized, isAuthorizedForRole } from "../../util/AuthorizationHelper";
import Unauthorized from "../auth/Unauthorized";
import { jwtDecode } from "jwt-decode";
import { UserModel } from "../../models/UserModel";
import PackageModel from "../../models/PackageModel";
import CloseIcon from '@mui/icons-material/Close';
import UserReportDetails from "./UserReportDetails";
import PackageReportDetails from "./PackageReportDetails";

export default function Reports() {
    const jwt = sessionStorage["jwt"];

    const [employees, setEmployees] = useState<UserModel[]>([]);
    const [clients, setClients] = useState<UserModel[]>([]);
    const [packages, setPackages] = useState<PackageModel[]>([]);
    const [detailsFor, setDetailsFor] = useState<string>("None");

    const [open, setOpen] = useState<boolean>(false);

    const { Role } = jwt != null ? jwtDecode(jwt) : { Role: "None" } as any;

    if (!isAuthorized(jwt) || isAuthorized(jwt) && !isAuthorizedForRole(Role, "Admin")) {
        return (
            <Unauthorized />
        )
    }

    function onClick(e: SyntheticEvent) {
        e.preventDefault();
        var anchor = e.target as HTMLAnchorElement;
        const url = anchor.getAttribute("href") as string;

        axios({
            method: "GET",
            url: url,
            headers: {
                "Authorization": `Bearer ${jwt}`
            }
        })
            .then((response) => {
                const data = response.data.data;
                const dataFor = response.data.dataFor;

                if (response.status == 200) {
                    switch (dataFor) {
                        case "Employees":
                            if (employees.length == 0 && data.length > 0) {
                                setEmployees(data);
                            }
                            break;
                        case "Clients":
                            if (clients.length == 0 && data.length > 0) {
                                setClients(data);
                            }
                            break;
                        case "Packages":
                            if (packages.length == 0 && data.length > 0) {
                                setPackages(data)
                            }
                    }

                    setOpen(true);
                    setDetailsFor(dataFor);
                }

            })

    }

    function renderReportDetails(): any {

        switch (detailsFor) {
            case "Employees":
                return <UserReportDetails users={employees} detailsFor={detailsFor} />
            case "Clients":
                return <UserReportDetails users={clients} detailsFor={detailsFor} />
            case "Packages":
                return <PackageReportDetails packages={packages} />
        }

    }

    function handleClose() {
        setOpen(false);
    }
    return (
        <>
            <Box sx={{
                display: "flex",
                justifyContent: "center",
                width: "100%",
                marginTop: "5%"
            }}>
                <Typography style={{ width: "100%", display: "inline-flex", justifyContent: "center" }}>
                    <Link variant="h6" underline="hover" href={`${API_URL}/Reports/AllEmployees`} onClick={onClick}>
                        All Employees
                    </Link>
                    <Link variant="h6" underline="hover" href={`${API_URL}/Reports/AllClients`} onClick={onClick} sx={{ marginLeft: "5%" }}>
                        All Clients
                    </Link>
                    <Link variant="h6" underline="hover" href={`${API_URL}/Reports/AllRegisteredPackages`} onClick={onClick} sx={{ marginLeft: "5%" }}>
                        All Registered Packages
                    </Link>
                </Typography>
                <Modal
                    open={open}
                    onClose={handleClose}
                    aria-labelledby="parent-modal-title"
                    aria-describedby="parent-modal-description">
                    <Box sx={{ ...MODAL_STYLE }}>
                        <CloseIcon onClick={handleClose} sx={{
                            float: "right",
                            cursor: "pointer"
                        }}>
                        </CloseIcon>
                        {renderReportDetails()}
                    </Box>
                </Modal>
            </Box>
        </>
    )
}