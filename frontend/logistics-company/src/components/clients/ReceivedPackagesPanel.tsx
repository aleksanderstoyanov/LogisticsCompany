import { useEffect, useState } from "react";
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { Box } from "@mui/material";

export default function ReceivedPackagesPanel() {
    const jwt = sessionStorage["jwt"];
    const [user, setUser] = useState<UserModel>(new UserModel(0, "", "Anonymous"))
    const [rows, setRows] = useState<any[]>([]);

    const columns: GridColDef[] = [
        {
            field: "id",
            editable: false,
            width: 200,
            headerName: "Id"
        },
        {
            field: "address",
            headerName: "Address",
            width: 200,
            editable: false
        },
        {
            field: "packageStatusName",
            headerName: "Status",
            width: 200,
            editable: false
        },
        {
            field: "weight",
            headerName: "Weight",
            width: 200,
            editable: false
        },
        {
            field: "toOffice",
            headerName: "To Office",
            type: "boolean",
            editable: false
        },
    ]

    useEffect(() => {
        if (jwt != null) {
            const API_URL = "https://localhost:7209/api";

            const { Id, Role, Email } = jwtDecode(jwt) as any;

            setUser((userModel: UserModel) => {
                user.id = Id;
                user.email = Email;
                user.role = Role;

                return userModel;
            })

            if (user.role == "Client") {
                axios({
                    method: "GET",
                    url: `${API_URL}/Packages/GetReceived?id=${user.id}`,
                    headers: {
                        "Authorization": `Bearer ${jwt}`
                    }
                })
                    .then((response) => {
                        const data = response.data;
                        if (rows.length == 0 && data.length > 0) {
                            setRows(data);
                        }
                    })
            }

        }
    })
    return (
        <Box sx={{
            height: 400,
            width: '100%',
            marginTop: "7%"
        }}>
            <DataGrid rows={rows}
                columns={columns}
                initialState={{
                    pagination: {
                        paginationModel: {
                            pageSize: 5,
                        },
                    },
                }}
                pageSizeOptions={[5]} />
        </Box>
    )
}