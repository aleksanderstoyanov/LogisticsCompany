import { useEffect, useState } from "react";
import { UserModel } from "../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import { Box, Button, Typography } from "@mui/material";
import { DataGrid, GridActionsCellItem, GridColDef, GridEventListener, GridRowEditStopReasons, GridRowId, GridRowModel, GridRowModes, GridRowModesModel, GridRowsProp, GridSlots, GridToolbarContainer } from "@mui/x-data-grid";
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/DeleteOutlined';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';
import axios from "axios";

export default function OfficePanel() {
  const API_URL = "https://localhost:7209/api";

  const [userModel, setUserModel] = useState<UserModel>(new UserModel(0, "Anonymous", "None"));
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
  function getRandomInt(min: number, max: number) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  const processRowUpdate = (newRow: GridRowModel) => {

    const foundRow = rows.find((row) => row.id === newRow.id) as GridRowModel;
    let updatedRow = { ...newRow, isNew: newRow.isNew ? true : false };
    var isCreated = false;
    if (newRow.isNew) {
      axios({
        method: "POST",
        url: `${API_URL}/Offices/Create`,
        data: updatedRow,
        headers: {
          "Authorization": `Bearer ${jwt}`
        }
      })
        .then((response) => {
          console.log(response);
          if (response.status == 200) {

            setRows(rows.map((row) => {
              if (row.id === newRow.id) {
                row.id = response.data.id;
                row.address = newRow.address;
                return row;
              }
              else {
                return (row.id === newRow.id ? updatedRow : row)
              }
            }));
          }
        })

    }

    else if (foundRow != null && !deepEqual(foundRow, newRow)) {
      axios({
        method: "PUT",
        url: `${API_URL}/Offices/Update`,
        data: updatedRow,
        headers: {
          "Authorization": `Bearer ${jwt}`
        }
      })

      setRows(rows.map((row) => {
        return (row.id === newRow.id ? updatedRow : row)
      }));
    }
    return updatedRow;

  };

  function onSave(id: GridRowId) {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } })
  }

  function onDelete(id: GridRowId) {
    axios({
      method: "DELETE",
      url: `${API_URL}/Offices/Delete?id=${id}`,
      headers: {
        "Authorization": `Bearer ${jwt}`
      }
    })
      .then(function (response) {
        if (response.status == 200) {
          setRows(rows.filter(row => row.id != id));
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
          url: `${API_URL}/Offices/GetAll`,
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

    if (Role != "Admin") {
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
  interface EditToolbarProps {
    setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
    setRowModesModel: (
      newModel: (oldModel: GridRowModesModel) => GridRowModesModel,
    ) => void;
  }
  function EditToolbar(props: EditToolbarProps) {
    const { setRows, setRowModesModel } = props;

    const handleClick = () => {
      const id = getRandomInt(rows.length, Number.MAX_VALUE);
      setRows((oldRows) => [...oldRows, { id, address: '', isNew: true }]);
      setRowModesModel((oldModel) => ({
        ...oldModel,
        [id]: { mode: GridRowModes.Edit, fieldToFocus: 'name' },
      }));
    };

    return (
      <GridToolbarContainer>
        <Button color="primary" startIcon={<AddIcon />} onClick={handleClick}>
          Add record
        </Button>
      </GridToolbarContainer>
    );
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
        slots={{
          toolbar: EditToolbar as GridSlots['toolbar'],
        }}
        slotProps={{
          toolbar: { setRows, setRowModesModel },
        }}
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