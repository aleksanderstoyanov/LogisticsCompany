import { useEffect, useState } from "react";
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import { Box, Typography } from "@mui/material";
import { DataGrid, GridActionsCellItem, GridColDef, GridEventListener, GridRowEditStopReasons, GridRowId, GridRowModel, GridRowModes, GridRowModesModel, GridRowsProp } from "@mui/x-data-grid";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/DeleteOutlined';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';
import axios from "axios";

export default function AdminPanel() {

  const API_URL = "https://localhost:7209/api";

  const [userModel, setUserModel] = useState<UserModel>(new UserModel("Anonymous", "None"));
  const [rows, setRows] = useState<any[]>([]);
  const [rowModesModel, setRowModesModel] = useState<GridRowModesModel>({});

  const jwt = sessionStorage["jwt"];

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
        data: updatedRow,
        url: `${API_URL}/Users/Update`,
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
      url: `${API_URL}/Users/Delete?id=${id}`,
      headers: {
        "Authorization": `Bearer ${jwt}`
      }
    })
      .then((response) => {
        if(response.status == 200){
            setRows(rows.filter(row => row.id != id))
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
      field: 'firstName',
      width: 200,
      headerName: 'First Name',
      editable: true
    },
    {
      field: 'lastName',
      width: 200,
      headerName: 'Last Name',
      editable: true
    },
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
      type: 'singleSelect',
      valueOptions: ['Client', 'OfficeEmployee', 'Courier'],
      editable: true,
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
            icon={<DeleteIcon />}
            label="Delete"
            onClick={() => { onDelete(id) }}
            color="inherit"
          />,
        ];
      },
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
            if (rows.length == 0 && data.length > 0) {
              setRows(data);
            }
          })
      }

    }
  });

  if (jwt != null) {

        const { Role } = jwtDecode(jwt) as any;

        if(Role != "Admin"){
          return(
            <Box sx={{
               height: 400,
               width: "100%",
               marginTop:"7%"
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
  else if(jwt == null){
    return(
      <Box sx={{
         height: 400,
         width: "100%",
         marginTop:"7%"
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
        checkboxSelection
        disableRowSelectionOnClick
      />
    </Box>
  );
}