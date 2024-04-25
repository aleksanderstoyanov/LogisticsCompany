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
import { log } from "console";

export default function Packages() {
    const API_URL = "https://localhost:7209/api";

    const [userModel, setUserModel] = useState<UserModel>(new UserModel(0, "Anonymous", "None"));
    const [users, setUsers] = useState<UserModel[]>([]);
    const [rows, setRows] = useState<any[]>([]);
    const [offices, setOffices] = useState<any[]>([]);

    const [rowModesModel, setRowModesModel] = useState<GridRowModesModel>({});
    const [isEditable, setEditable] = useState<boolean>(false);

    const jwt = sessionStorage["jwt"];

    const statuses = {
        "Courier": ["InDelivery", "Delivered"],
        "OfficeEmployee": ["NonRegistered", "Registered"]
    };

    useEffect(() => {
        if (jwt != null) {

            const { Id, Email, Role } = jwtDecode(jwt) as any;

            setUserModel((userModel: UserModel) => {
                userModel.id = Id;
                userModel.email = Email;
                userModel.role = Role;
                return userModel;
            })


            if (Role == "OfficeEmployee") {
                setEditable(true);
            }
            if (Role == "OfficeEmployee" || Role == "Courier") {
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

                if (users.length == 0) {
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
                                console.log(data);

                                setUsers(data);
                            }
                        })
                }
            }

        }
    });

    if (jwt != null) {

        const { Role } = jwtDecode(jwt) as any;

        if (Role != "OfficeEmployee" && Role != "Courier") {
            return (
                <Box sx={{
                    height: 400,
                    width: "100%",
                    marginTop: "7%"
                }}>
                    <Typography variant="h4" sx={{
                        display: "flex",
                        justifyContent: "center",
                        alignContent: "center"
                    }}>
                        You do not have permisson for this page!
                    </Typography>
                </Box>
            )
        }
    }
    else if (jwt == null) {
        return (
            <Box sx={{
                height: 400,
                width: "100%",
                marginTop: "7%"
            }}>
                <Typography variant="h4" sx={{
                    display: "flex",
                    justifyContent: "center",
                    alignContent: "center"
                }}>
                    You do not have permisson for this page!
                </Typography>
            </Box>
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

    function deepEqual(x: any, y: any): boolean {
        return (x && y && typeof x === 'object' && typeof y === 'object') ?
            (Object.keys(x).length === Object.keys(y).length) &&
            Object.keys(x).reduce(function (isEqual: boolean, key: any) {
                return isEqual && deepEqual(x[key], y[key]);
            }, true) : (x === y);
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
                .then((response) => {

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
            if(response.status == 200){
                setTimeout(() => {
                    setRows(rows.filter(row => row.id != id));
                });
            }
        })
    }
    const columns: GridColDef[] = [
        {
            field: 'id',
            headerName: 'ID',
            width: 200,
            editable: false
        },
        {
            field: 'address',
            width: 200,
            headerName: 'Address',
            editable: isEditable
        },
        {
            field: 'fromId',
            width: 200,
            headerName: 'From',
            type: "singleSelect",
            valueOptions: users.map((user) => {
                return {
                    value: user.id,
                    label: user.email
                }
            }),
            editable: isEditable
        },
        {
            field: 'toId',
            width: 200,
            headerName: 'To',
            editable: isEditable,
            type: "singleSelect",
            valueOptions: users.map((user) => {
                return {
                    value: user.id,
                    label: user.email
                }
            }),
        },
        {
            field: 'packageStatusName',
            width: 200,
            headerName: 'Package Status',
            type: 'singleSelect',
            valueOptions: statuses[userModel.role == "OfficeEmployee" ? "OfficeEmployee" : "Courier"],
            editable: true,
        },
        {
            field: 'toOffice',
            width: 200,
            headerName: 'To Office',
            type: 'boolean',
            editable: userModel.role == "OfficeEmployee" ? true : false,
        },
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
    ];


    return (
        <Box sx={{
            height: 400,
            width: '100%',
            marginTop: "7%"
        }}>
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