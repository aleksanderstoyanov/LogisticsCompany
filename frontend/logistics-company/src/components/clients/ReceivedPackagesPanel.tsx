import { useEffect, useState } from "react";
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { Box } from "@mui/material";
import { ColumnContainer } from "../../util/ColumnContainer";
import { isAuthorized, isAuthorizedForRole } from "../../util/AuthorizationHelper";
import Unauthorized from "../auth/Unauthorized";
import { API_URL, DEFAULT_USER_EMAIL, DEFAULT_USER_ID, DEFAULT_USER_Role, GRID_BOX_STYLE } from "../../util/Constants";

export default function ReceivedPackagesPanel() {
    const jwt = sessionStorage["jwt"];
    const [user, setUser] = useState<UserModel>(
        new UserModel(
            DEFAULT_USER_ID,
            DEFAULT_USER_EMAIL,
            DEFAULT_USER_Role
        )
    );
    const [rows, setRows] = useState<any[]>([]);
    const { Id, Email, Role } = isAuthorized(jwt) ? jwtDecode(jwt)
    : { Id: null, Role: null, Email: null } as any;

    let columns = new ColumnContainer()
        .Add("id", "Id", 200, false)
        .Add("address", "Address", 200, false)
        .Add("packageStatusName", "Status", 200, false)
        .Add("weight", "Weight", 200, false)
        .Add("toOffice", "To Office", 200, false, "boolean")
        .GetColumns()

    useEffect(() => {
        if (isAuthorized(jwt)) {
            const { Id, Role, Email } = jwtDecode(jwt) as any;

            setUser((userModel: UserModel) => {
                user.id = Id;
                user.email = Email;
                user.role = Role;

                return userModel;
            })

            if (isAuthorizedForRole(user.role, "Client")) {
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

    if (!isAuthorizedForRole(Role, "Client") || !isAuthorized(jwt))
        return (
            <Unauthorized />
        )

    return (
        <Box sx={GRID_BOX_STYLE}>
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