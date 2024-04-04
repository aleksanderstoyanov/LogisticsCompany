import { useEffect, useState } from "react";
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import { Box } from "@mui/material";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import axios from "axios";

export default function AdminPanel() {

    const API_URL = "https://localhost:7209/api";

    const [userModel, setUserModel] = useState<UserModel>(new UserModel("Anonymous", "None"));
    const [rows, setRows] = useState<[]>([]);

    const jwt = sessionStorage["jwt"];

    const columns: GridColDef<(typeof rows)[number]>[] = [
        { field: 'id', headerName: 'ID', width: 200 },
        {
            field: 'email',
            width: 200,
            headerName: 'Email Address',
            editable: true,
        },
        {
            field: 'roleName',
            width: 200,
            headerName: 'Role',
            editable: true,
        }
    ];

    useEffect(() => {
        if (jwt != null) {

            const { Email, Role } = jwtDecode(jwt) as any;

            setUserModel((userModel: UserModel) => {

                userModel.email = Email;
                userModel.role = Role;
                return userModel;
            })

            if (Role == "Admin") {
                axios({
                    method: "GET",
                    url: `${API_URL}/Users/GetAll`,
                    headers: {
                        "Authorization": `Bearer ${jwt}`
                    }
                })
                    .then(function (response) {
                        var data = response.data;
                        if (rows.length == 0) {
                            setRows(data);
                        }
                    })
            }

        }
    });

    return (
        <Box sx={{
            height: 400,
            width: '100%',
            marginTop: "7%"
        }}>
            <DataGrid
                rows={rows}
                columns={columns}
                initialState={{
                    pagination: {
                        paginationModel: {
                            pageSize: 5,
                        },
                    },
                }}
                pageSizeOptions={[5]}
                checkboxSelection
                disableRowSelectionOnClick
            />
        </Box>
    );
}