import { useEffect, useState } from "react";
import { UserModel } from "../../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import { Box } from "@mui/material";
import { DataGrid, GridActionsCellItem, GridEventListener, GridRowEditStopReasons, GridRowId, GridRowModel, GridRowModes, GridRowModesModel, GridRowsProp } from "@mui/x-data-grid";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/DeleteOutlined';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';
import { DEFAULT_USER_EMAIL, DEFAULT_USER_ID, DEFAULT_USER_Role, GRID_BOX_STYLE, USER_ROLES } from "../../../util/Constants";
import { deepEqual } from "../../../util/Common";
import { ColumnContainer } from "../../../util/ColumnContainer";
import { isAuthorized, isAuthorizedForRole } from "../../../util/AuthorizationHelper";
import Unauthorized from "../../auth/Unauthorized";
import { deleteUser, getAllUsers, updateUser } from "../../../requests/UserRequests";
import { getAllOffices } from "../../../requests/OfficeRequests";

export default function AdminPanel() {
  const [userModel, setUserModel] = useState<UserModel>(
    new UserModel(
      DEFAULT_USER_ID,
      DEFAULT_USER_EMAIL,
      DEFAULT_USER_Role
    )
  );

  const [rows, setRows] = useState<any[]>([]);
  const [offices, setOffices] = useState<any[]>([]);
  const [rowModesModel, setRowModesModel] = useState<GridRowModesModel>({});

  const jwt = sessionStorage["jwt"];

  const { Email, Role } = isAuthorized(jwt) ? jwtDecode(jwt)
    : { Role: null, Email: null } as any;

  useEffect(() => {
    if (isAuthorized(jwt)) {

      setUserModel((userModel: UserModel) => {
        userModel.email = Email;
        userModel.role = Role;
        return userModel;
      })

      if (isAuthorizedForRole(Role, "Admin")) {
        getAllUsers(jwt)
          .then(function (response) {
            var data = response.data;
            if (rows.length == 0 && data.length > 0) {
              setRows(data);
            }
          })

        getAllOffices(jwt)
          .then(function (response) {
            var data = response.data;
            if (offices.length == 0 && data.length > 0) {
              setOffices(data.map((office: any) => office.address));
            }
          })
      }

    }
  });

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
      updateUser(jwt, updatedRow);
    }
    setRows(rows.map((row) => (row.id === newRow.id ? updatedRow : row)));

    return updatedRow;
  };

  function onSave(id: GridRowId) {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } })
  }

  function onDelete(id: GridRowId) {
    deleteUser(id, jwt)
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
    .Add("firstName", "First Name", 200, true)
    .Add("lastName", "Last Name", 200, true)
    .Add("email", "Email", 200, true)
    .Add("roleName", "Role", 200, true, "singleSelect", USER_ROLES)
    .Add("officeName", "Office", 200, true, "singleSelect", offices)
    .GetColumns();

  columns.push({
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
          icon={<DeleteIcon />}
          label="Delete"
          onClick={() => { onDelete(id) }}
          color="inherit"
        />,
      ];
    },
  })

  if (!isAuthorized(jwt) || Role != "Admin") {
    return (
      <Unauthorized />
    )
  }

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
        disableRowSelectionOnClick
      />
    </Box>
  );
}