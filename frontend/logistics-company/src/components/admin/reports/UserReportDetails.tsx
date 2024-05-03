import * as React from 'react';
import { Divider, List, ListItem, ListItemAvatar, ListItemIcon, ListItemText, Typography } from "@mui/material";
import BadgeIcon from '@mui/icons-material/Badge';
import RecordVoiceOverIcon from '@mui/icons-material/RecordVoiceOver';
import { getRandomInt } from '../../../util/Common';

export default function UserReportDetails(props: any) {
    return (
        <List
            sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
            <Typography variant="h6">
                {props.detailsFor == "Employees" ? "Employee Reports" : "Client Reports"}
            </Typography>
            {props.users.map((user: any) => {

                return (
                    <>
                        <ListItem key={props.users.indexOf(user)} alignItems="flex-start">
                            <ListItemIcon>
                                {props.detailsFor == "Employees" ? <BadgeIcon /> : <RecordVoiceOverIcon />}
                            </ListItemIcon>
                            <ListItemText
                                primary={`${user.firstName} ${user.lastName}`}
                                secondary={
                                    <React.Fragment>
                                        <Typography
                                            sx={{ display: 'inline' }}
                                            component="span"
                                            variant="body2"
                                            color="text.primary"
                                        >
                                        </Typography>
                                        {`Role — ${user.roleName}`}
                                    </React.Fragment>
                                }
                            />
                        </ListItem>
                        <Divider variant="inset" component="li" />
                    </>
                )
            })}
        </List>
    );
}