import { useEffect, useState } from "react";
import { UserModel } from "../../../models/UserModel";
import { jwtDecode } from "jwt-decode";
import { Box, Button, Typography } from "@mui/material";
import { DataGrid, GridActionsCellItem, GridColDef, GridEventListener, GridRowEditStopReasons, GridRowId, GridRowModel, GridRowModes, GridRowModesModel, GridRowsProp, GridSlots, GridToolbarContainer } from "@mui/x-data-grid";
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/DeleteOutlined';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';
import axios from "axios";
import { ColumnContainer } from "../../../util/ColumnContainer";
import { isAuthorized, isAuthorizedForRole } from "../../../util/AuthorizationHelper";
import { API_URL, DEFAULT_USER_EMAIL, DEFAULT_USER_ID, DEFAULT_USER_Role, GRID_BOX_STYLE } from "../../../util/Constants";
import { deepEqual, getRandomInt } from "../../../util/Common";
import Unauthorized from "../../auth/Unauthorized";

export default function OfficePanel() {
  const [userModel, setUserModel] = useState<UserModel>(
    new UserModel(
      DEFAULT_USER_ID,
      DEFAULT_USER_EMAIL,
      DEFAULT_USER_Role
    )
  );
  const [rows, setRows] = useState<any[]>([]);
  const [rowModesModel, setRowModesModel] = useState<GridRowModesModel>({});

  const jwt = sessionStorage["jwt"];
  const { Email, Role } = isAuthorized(jwt) ? jwtDecode(jwt)
    : { Role: null, Email: null } as any;

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
    let updatedRow = { ...newRow, isNew: newRow.isNew ? true : false };

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
          if (response.status == 200) {

            setRows(rows.map((row) => {
              if (row.id === newRow.id) {
                row.id = response.data.id;
                row.address = newRow.address;
                row.pricePerWeight = newRow.pricePerWeight;
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
          setTimeout(() => {
            setRows(rows.filter(row => row.id != id));
          });
        }
      })
  }

  let columns = new ColumnContainer()
    .Add("id", "ID", 200, false)
    .Add("address", "Address", 200, true)
    .Add("pricePerWeight", "Price Per Weight", 200, true, "number")
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
            icon={<DeleteIcon />}
            label="Delete"
            onClick={() => { onDelete(id) }}
            color="inherit"
          />,
        ];
      },
    });

  useEffect(() => {
    if (isAuthorized(jwt)) {

      setUserModel((userModel: UserModel) => {
        userModel.email = Email;
        userModel.role = Role;
        return userModel;
      })

      if (isAuthorizedForRole(Role, "Admin")) {
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

  if (!isAuthorized(jwt) || !isAuthorizedForRole(Role, "Admin"))
    return (
      <Unauthorized />
    )

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
    <Box sx={GRID_BOX_STYLE}>
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
        disableRowSelectionOnClick
      />
    </Box>
  );
}