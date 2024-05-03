import { Divider, List, ListItem, ListItemIcon, ListItemText, Typography } from "@mui/material";
import Inventory2Icon from '@mui/icons-material/Inventory2';
import React from "react";
import { PackageStatusReportModel } from "../../../models/PackageStatusReportModel";

export default function PackageReportDetails(props: any) {

    const detailsFor = props.detailsFor;

    function renderDetailsFor(logisticPackage: any): React.ReactNode {
        switch (detailsFor) {
            case "ReceivedPackages":
                return (
                    <React.Fragment>
                        <Typography
                            sx={{ display: 'block' }}
                            component="span"
                            paragraph={true}
                            variant="body2"
                            color="text.primary">
                        </Typography>
                        {`Address — ${logisticPackage.address}`}
                        <Typography
                            sx={{ display: 'block' }}
                            component="span"
                            paragraph={true}
                            variant="body2"
                            color="text.primary">
                        </Typography>
                        {`From — ${logisticPackage.fromUser}`}
                    </React.Fragment>)

            case "SentPackages":
                return (
                    <React.Fragment>
                        <Typography
                            sx={{ display: 'block' }}
                            component="span"
                            paragraph={true}
                            variant="body2"
                            color="text.primary">
                        </Typography>
                        {`Address — ${logisticPackage.address}`}
                        <Typography
                            sx={{ display: 'block' }}
                            component="span"
                            paragraph={true}
                            variant="body2"
                            color="text.primary">
                        </Typography>
                        {`To — ${logisticPackage.toUser}`}
                    </React.Fragment>)
            default:
                return (
                    <React.Fragment>
                        <Typography
                            sx={{ display: 'block' }}
                            component="span"
                            paragraph={true}
                            variant="body2"
                            color="text.primary">
                        </Typography>
                        {`Address — ${logisticPackage.address}`}
                        <Typography
                            sx={{ display: 'block' }}
                            component="span"
                            paragraph={true}
                            variant="body2"
                            color="text.primary">
                        </Typography>
                        {`From — ${logisticPackage.fromUser}`}
                        <Typography
                            sx={{ display: 'block' }}
                            component="span"
                            paragraph={true}
                            variant="body2"
                            color="text.primary">
                        </Typography>
                        {`To — ${logisticPackage.toUser}`}
                    </React.Fragment>)
        }
    }
    return (
        <List
            sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
            <Typography variant="h6">
                Packages Reports
            </Typography>
            {props.packages.map((logisticPackage: PackageStatusReportModel) => {

                return (
                    <>
                        <ListItem key={props.packages.indexOf(logisticPackage)} alignItems="flex-start">
                            <ListItemIcon>
                                <Inventory2Icon />
                            </ListItemIcon>
                            <ListItemText
                                primary={`Status: ${logisticPackage.packageStatusName}`}
                                secondary={
                                    renderDetailsFor(logisticPackage)
                                }
                            />
                        </ListItem>
                        <Divider variant="inset" component="li" />
                    </>
                )
            })}
        </List >
    );
}