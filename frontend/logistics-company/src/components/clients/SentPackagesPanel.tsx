import { useEffect, useState } from "react";
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import { Box } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import { ColumnContainer } from "../../util/ColumnContainer";
import { isAuthorized, isAuthorizedForRole } from "../../util/AuthorizationHelper";
import Unauthorized from "../auth/Unauthorized";
import { DEFAULT_USER_EMAIL, DEFAULT_USER_ID, DEFAULT_USER_Role, GRID_BOX_STYLE } from "../../util/Constants";
import { getSentPackages } from "../../requests/PackageRequests";

export default function SentPackagesPanel() {
    const [user, setUser] = useState<UserModel>(
        new UserModel(
            DEFAULT_USER_ID,
            DEFAULT_USER_EMAIL,
            DEFAULT_USER_Role
        )
    );
    const [rows, setRows] = useState<any[]>([]);

    const jwt = sessionStorage["jwt"];
    const { Id, Email, Role } = isAuthorized(jwt) ? jwtDecode(jwt)
        : { Id: null, Role: null, Email: null } as any;

    let columns = new ColumnContainer()
        .Add("id", "Id", 200, false)
        .Add("address", "Address", 200, false)
        .Add("packageStatusName", "Status", 200, false)
        .Add("weight", "Weight", 200, false)
        .Add("toOffice", "To Office", 200, false, "boolean")
        .GetColumns();

    useEffect(() => {
        if (isAuthorized(jwt)) {
            setUser((userModel: UserModel) => {
                userModel.id = Id;
                userModel.email = Email;
                userModel.role = Role;
                return userModel;
            })
            getSentPackages(jwt, user.id)
                .then((response) => {
                    const data = response.data;

                    if (rows.length == 0 && data.length > 0) {
                        setRows(data);
                    }
                })
        }
    })

    if (!isAuthorizedForRole(Role, "Client") || !isAuthorized(jwt))
        return (
            <Unauthorized />
        )

    return (
        <Box sx={GRID_BOX_STYLE}>
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