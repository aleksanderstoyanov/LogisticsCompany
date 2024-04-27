import { useEffect, useState } from "react";
import { UserModel } from "../../models/UserModel";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/DeleteOutlined';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';
import { DataGrid, GridActionsCellItem, GridColDef, GridEventListener, GridRowEditStopReasons, GridRowId, GridRowModel, GridRowModes, GridRowModesModel } from "@mui/x-data-grid";
import axios from "axios";
import { jwtDecode } from "jwt-decode";
import { Box, Typography } from "@mui/material";
import Unauthorized from "../auth/Unauthorized";
import { API_URL, DEFAULT_USER_EMAIL, DEFAULT_USER_ID, DEFAULT_USER_Role, GRID_BOX_STYLE, PACKAGE_STATUSES } from "../../util/Constants";
import { isAuthorized, isAuthorizedForRole } from "../../util/AuthorizationHelper";
import { deepEqual, valueOptionMapper } from "../../util/Common";
import { ColumnContainer } from "../../util/ColumnContainer";

export default function Packages() {
    const [userModel, setUserModel] = useState<UserModel>(
        new UserModel(
            DEFAULT_USER_ID,
            DEFAULT_USER_EMAIL,
            DEFAULT_USER_Role
        )
    );
    const [users, setUsers] = useState<UserModel[]>([]);
    const [rows, setRows] = useState<any[]>([]);
    const [offices, setOffices] = useState<any[]>([]);

    const [rowModesModel, setRowModesModel] = useState<GridRowModesModel>({});
    const [isEditable, setEditable] = useState<boolean>(false);

    const jwt = sessionStorage["jwt"];
    
    const { Id, Email, Role } = isAuthorized(jwt) ? jwtDecode(jwt) 
        : { Id: null, Role: null, Email: null } as any;

    const packageStatusValueOptions = PACKAGE_STATUSES[
        userModel.role == "OfficeEmployee" ? "OfficeEmployee" : "Courier"
    ];
    
    const userValueOptions = users.map((user) => { return valueOptionMapper(user, "id", "email") });

    useEffect(() => {
        if (isAuthorized(jwt)) {

            setUserModel((userModel: UserModel) => {
                userModel.id = Id;
                userModel.email = Email;
                userModel.role = Role;
                return userModel;
            })
        }

        if (isAuthorizedForRole(Role, "OfficeEmployee")) {
            setEditable(true);
        }

        if (isAuthorizedForRole(Role, "OfficeEmployee") || isAuthorizedForRole(Role, "Courier")) {
            if (rows.length == 0) {
                axios({
                    method: "GET",
                    url: `${API_URL}/Packages/GetAll`,
                    headers: {
                        "Authorization": `Bearer ${jwt}`
                    }
                })
                    .then(function (response) {
                        var data = response.data;
                        if (rows.length == 0 && data.length > 0) {
                            setRows(data);
                        }
                    })
            }

            if (users.length === 0) {
                axios({
                    method: "GET",
                    url: `${API_URL}/Users/GetAllExcept?id=${userModel.id}&role=Client`,
                    headers: {
                        "Authorization": `Bearer ${jwt}`
                    }
                })
                    .then((response) => {
                        const data = response.data;
                        if (users.length == 0 && data.length > 0) {
                            data.filter((user: any) => {
                                return user.roleName = "Client"
                            })
                            setUsers(data);
                        }
                    })
            }
        }
    });

    if (!isAuthorized(jwt) || isAuthorized(jwt) && !isAuthorizedForRole(Role, "OfficeEmployee") && !isAuthorizedForRole(Role, "Courier")) {
        return (
            <Unauthorized />
        )
    }

    const handleRowEditStop: GridEventListener<'rowEditStop'> = (params, event) => {
        if (params.reason === GridRowEditStopReasons.rowFocusOut) {
            event.defaultMuiPrevented = true;
        }
    };

    const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
        setRowModesModel(newRowModesModel);
    };

    function onEdit(id: GridRowId) {
        setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.Edit } })
    }

    function onCancel(id: GridRowId) {
        setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } })
    }

    const processRowUpdate = (newRow: GridRowModel) => {
        const foundRow = rows.find((row) => row.id === newRow.id) as GridRowModel;
        const updatedRow = { ...newRow, isNew: false };
        if (foundRow != null && !deepEqual(foundRow, newRow)) {
            axios({
                method: "PUT",
                url: `${API_URL}/Packages/Update`,
                data: updatedRow,
                headers: {
                    "Authorization": `Bearer ${jwt}`
                }
            })
        }
        setRows(rows.map((row) => (row.id === newRow.id ? updatedRow : row)));

        return updatedRow;
    };

    function onSave(id: GridRowId) {
        setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } })
    }

    function onDelete(id: GridRowId) {
        axios({
            method: "DELETE",
            url: `${API_URL}/Packages/Delete?id=${id}`,
            headers: {
                "Authorization": `Bearer ${jwt}`
            }
        })
            .then((response) => {
                if (response.status == 200) {
                    setTimeout(() => {
                        setRows(rows.filter(row => row.id != id));
                    });
                }
            })
    }

    let columns = new ColumnContainer()
        .Add("id", "ID", 200, false)
        .Add("address", "Address", 200, isEditable)
        .Add("fromId", "From", 200, isEditable, "singleSelect", userValueOptions)
        .Add("toId", "To", 200, isEditable, "singleSelect", userValueOptions)
        .Add("packageStatusName", "Status", 200, true, "singleSelect", packageStatusValueOptions)
        .GetColumns();

    columns.push(
        {
            field: 'actions',
            type: 'actions',
            headerName: 'Actions',
            width: 100,
            cellClassName: 'actions',
            getActions: ({ id }) => {
                const isInEditMode = rowModesModel[id]?.mode === GridRowModes.Edit;

                if (isInEditMode) {
                    return [
                        <GridActionsCellItem
                            icon={<SaveIcon />}
                            label="Save"
                            onClick={() => { onSave(id) }}
                            sx={{
                                color: 'primary.main',
                            }}
                        />,
                        <GridActionsCellItem
                            icon={<CancelIcon />}
                            onClick={() => { onCancel(id) }}
                            label="Cancel"
                            className="textPrimary"
                            color="inherit"
                        />,
                    ];
                }

                return [
                    <GridActionsCellItem
                        icon={<EditIcon />}
                        label="Edit"
                        onClick={() => { onEdit(id) }}
                        sx={{
                            color: 'primary.main',
                        }}
                    />,
                    <GridActionsCellItem
                        sx={{
                            display: isEditable ? "inline" : "none"
                        }}
                        icon={<DeleteIcon />}
                        label="Delete"
                        onClick={() => { onDelete(id) }}
                        color="inherit"
                    />,
                ];
            },
        }
    )

    return (
        <Box sx={GRID_BOX_STYLE}>
            <DataGrid
                rows={rows}
                editMode="row"
                columns={columns}
                onRowModesModelChange={handleRowModesModelChange}
                onRowEditStop={handleRowEditStop}
                processRowUpdate={processRowUpdate}
                rowModesModel={rowModesModel}
                initialState={{
                    pagination: {
                        paginationModel: {
                            pageSize: 5,
                        },
                    },
                }}
                pageSizeOptions={[5]}
                checkboxSelection={userModel.role == "Courier" ? true : false}
                disableRowSelectionOnClick
            />
        </Box>
    );
}