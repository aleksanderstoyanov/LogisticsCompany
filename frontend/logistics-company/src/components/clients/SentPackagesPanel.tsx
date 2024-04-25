import { useEffect, useState } from "react";
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { Box } from "@mui/material";
import { DataGrid, GridColDef } from "@mui/x-data-grid";

export default function SentPackagesPanel() {
    const API_URL = "https://localhost:7209/api";

    const [user, setUser] = useState<UserModel>(new UserModel(0, "", "Anonymous"));
    const [rows, setRows] = useState<any[]>([]);

    const jwt = sessionStorage["jwt"];

    const columns: GridColDef[] = [
        {
            field: "id",
            headerName: "ID",
            width: 200,
            editable: false
        },
        {
            field: "address",
            headerName: "Address",
            width: 200,
            editable: false
        },
        {
            field: "toOffice",
            headerName: "ToOffice",
            type: "boolean",
            width: 200
        },
        {
            field: "packageStatusName",
            headerName: "Status",
            editable: false,
            width: 200
        },
        {
            field: "weight",
            headerName: "Weight",
            width: 200,
            editable: false
        }
    ];
    useEffect(() => {
        if (jwt != null) {
            const { Id, Role, Email } = jwtDecode(jwt) as any;

            if (Role == "Client") {
                setUser((user: UserModel) => {
                    user.id = Id;
                    user.email = Email;
                    user.role = Role;

                    return user;
                })

                axios({
                    method: "GET",
                    url: `${API_URL}/Packages/GetSent?id=${user.id}`,
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
            marginTop: "7%",
            height: 400,
            width: "100%"
        }}>
            <DataGrid columns={columns} 
                      rows={rows}
                      initialState={
                        {
                            pagination: {
                                paginationModel: {
                                    page: 1,
                                    pageSize: 5
                                }
                            }
                        }
                      } 
            />
        </Box>
    )
}