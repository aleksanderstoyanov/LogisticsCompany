import { Box, Button, Typography } from "@mui/material";
import { API_URL, GRID_BOX_STYLE } from "../../util/Constants";
import { DataGrid, GridRowId, GridRowModel } from "@mui/x-data-grid";
import { SyntheticEvent, useEffect, useState } from "react";
import { isAuthorized, isAuthorizedForRole } from "../../util/AuthorizationHelper";
import LocalShippingOutlinedIcon from '@mui/icons-material/LocalShippingOutlined';
import { jwtDecode } from "jwt-decode";
import Unauthorized from "../auth/Unauthorized";
import axios from "axios";
import { ColumnContainer } from "../../util/ColumnContainer";

export default function Deliveries() {

    const [rows, setRows] = useState<any[]>([]);

    const jwt = sessionStorage["jwt"];

    const { Role } = jwt != null ? jwtDecode(jwt) : { Role: "None" } as any;


    useEffect(() => {
        debugger;
        if (isAuthorized(jwt) && isAuthorizedForRole("Courier", Role)) {
            axios({
                method: "GET",
                url: `${API_URL}/Deliveries/GetAll`,
                headers: {
                    "Authorization": `Bearer ${jwt}`
                }
            })
                .then((response) => {
                    const data = response.data;

                    if (rows.length == 0 && data.length > 0) {
                        debugger;
                        setRows(data);
                    }
                })
        }
    }, [])


    if (!isAuthorized(jwt) || !isAuthorizedForRole("Courier", Role)) {
        return <Unauthorized />
    }

    let columns = new ColumnContainer()
        .Add("id", "ID", 200, false)
        .Add("startDate", "Start Date", 200, false)
        .Add("endDate", "End Date", 200, false)
        .GetColumns();

    function onDeliver(event: SyntheticEvent, id: GridRowId) {
        var button = event.target as HTMLButtonElement;
        
        axios({
            url: `${API_URL}/Deliveries/Update`,
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${jwt}`
            },
            data: {id: id}
        })
        
        button.remove();
    }

    columns.push(
        {
            field: 'actions',
            type: 'actions',
            headerName: 'Actions',
            width: 300,
            cellClassName: 'actions',
            getActions: ({ id }) => {
                const row = rows.find(row => row.id == id);

                if (row.endDate != null) {
                    return [
                        <Typography variant="h6">
                            Delivery has finished!
                        </Typography>
                    ]
                }

                else {

                    return [
                        <Button
                            variant="outlined"
                            color="primary"
                            onClick={ (event) => {onDeliver(event, row.id)}}
                            endIcon={<LocalShippingOutlinedIcon />}>
                            Deliver
                        </Button>,
                    ];
                }
            },
        }
    )


    return (
        <Box sx={GRID_BOX_STYLE}
            width={500}>
            <DataGrid columns={columns}
                rows={rows}
                disableRowSelectionOnClick={true} />
        </Box>
    )
}