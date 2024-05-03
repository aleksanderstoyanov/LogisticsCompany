import { Divider, List, ListItem, ListItemIcon, ListItemText, Typography } from "@mui/material";
import Inventory2Icon from '@mui/icons-material/Inventory2';
import React from "react";
import { PackageStatusReportModel } from "../../../models/PackageStatusReportModel";

export default function PackageReportDetails(props: any) {
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
                                    </React.Fragment>
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