import { Box, Button, Checkbox, Container, Grid, InputLabel, MenuItem, Select, SelectChangeEvent, TextField, Typography } from "@mui/material";
import PackageModel from "../../models/PackageModel";
import { ChangeEvent, SyntheticEvent, useEffect, useState } from "react";
import Inventory2OutlinedIcon from '@mui/icons-material/Inventory2Outlined';
import InventoryIcon from '@mui/icons-material/Inventory';
import axios from "axios";
import { UserModel } from "../../models/UserModel";
import { API_URL, DEFAULT_PACKAGE_ADDRESS, DEFAULT_PACKAGE_FROM_ID, DEFAULT_PACKAGE_TO_ID, DEFAULT_PACKAGE_TO_OFFICE, DEFAULT_PACKAGE_WEIGHT, PACKAGE_FORM_IDS } from "../../util/Constants";
import { OfficeModel } from "../../models/OfficeModel";

export default function PackageForm(props: any) {
    const jwt = sessionStorage["jwt"];
    const [packageModel, changePackageModel] = useState<PackageModel>(
        new PackageModel(
            DEFAULT_PACKAGE_ADDRESS,
            DEFAULT_PACKAGE_FROM_ID,
            DEFAULT_PACKAGE_TO_ID,
            DEFAULT_PACKAGE_WEIGHT,
            DEFAULT_PACKAGE_TO_OFFICE,
            props.officeId
        )
    );
    const IDS = PACKAGE_FORM_IDS;
    const [ADDRESS_ID, TO_ID, WEIGHT_ID, TO_OFFICE] = IDS;

    useEffect(() => {
        changePackageModel((packageModel: PackageModel) => {
            packageModel.fromId = props.userId;
            return packageModel;
        })
    })

    function onChange(event: SyntheticEvent) {
        let target = event.target as HTMLInputElement;
        let value = target.value;
        const id = target.getAttribute("id");

        switch (id) {
            case ADDRESS_ID:
                changePackageModel((packageModel: PackageModel) => {
                    packageModel.address = value;
                    return packageModel;
                })
                break;
            case TO_ID:
                changePackageModel((packageModel: PackageModel) => {
                    packageModel.address = value;
                    return packageModel;
                })
                break;
            case WEIGHT_ID:
                changePackageModel((packageModel: PackageModel) => {
                    packageModel.weight = parseInt(value);
                    return packageModel;
                })
                break;
        }

    }

    function onCancel(event: SyntheticEvent) {
        IDS.forEach((id) => {
            let element = document.getElementById(id) as HTMLInputElement;
            element.value = "";
        })

        changePackageModel(new PackageModel(
            DEFAULT_PACKAGE_ADDRESS,
            DEFAULT_PACKAGE_FROM_ID,
            DEFAULT_PACKAGE_TO_ID,
            DEFAULT_PACKAGE_WEIGHT,
            DEFAULT_PACKAGE_TO_OFFICE,
            props.officeId
        ));
    }
    function onChangeUser(event: SelectChangeEvent) {
        const toValue = event.target.value;

        changePackageModel((packageModel: PackageModel) => {
            packageModel.toId = parseInt(toValue);
            return packageModel;
        })

    }

    function onChangeOffice(event: ChangeEvent<HTMLInputElement>) {
        const checked = event.target.checked;

        changePackageModel((packageModel: PackageModel) => {
            packageModel.toOffice = checked;
            return packageModel;
        })
    }

    function onSubmit(event: SyntheticEvent) {
        debugger;
        axios({
            method: "POST",
            url: `${API_URL}/Packages/Create`,
            data: packageModel,
            headers: {
                "Authorization": `Bearer ${jwt}`
            }
        })
            .then((response) => {
                if (response.status == 200) {
                    window.location.href = "/offices";
                }
            })
    }

    return (
        <Container maxWidth="sm">
            <Grid container justifyContent="center" alignItems="center" style={{ minHeight: "70vh" }}>
                <Grid>
                    <Box
                        display="flex"
                        alignItems="center"
                        justifyContent="center"
                        gap={2}>
                        <Typography variant="h4" component="h1" align="center">
                            Package Information
                            <Inventory2OutlinedIcon></Inventory2OutlinedIcon>
                        </Typography>
                    </Box>
                    <TextField
                        id={ADDRESS_ID}
                        fullWidth
                        margin="normal"
                        label="Address"
                        variant="outlined"
                        onChange={onChange}
                    />
                    <TextField
                        inputProps={{ type: "number" }}
                        id={WEIGHT_ID}
                        fullWidth
                        margin="normal"
                        label="Weight"
                        variant="outlined"
                        onChange={onChange}
                    />
                    <InputLabel id={TO_ID}>To</InputLabel>
                    <Select
                        fullWidth
                        labelId="to"
                        id={TO_ID}
                        onChange={onChangeUser}
                        defaultValue={""}
                    >
                        {
                            props.users.map((user: UserModel) => {
                                return (
                                    <MenuItem key={user.id} value={user.id}>
                                        {user.email}
                                    </MenuItem>
                                )
                            })
                        }
                    </Select>
                    <InputLabel id={TO_OFFICE}>To Office</InputLabel>
                    <Checkbox id={TO_OFFICE}
                        onChange={onChangeOffice}
                        icon={<Inventory2OutlinedIcon />}
                        checkedIcon={<InventoryIcon />} />
                    <Grid container justifyContent="center" spacing={2} style={{ marginTop: 24 }}>
                        <Grid item>
                            <Button variant="outlined" onClick={onCancel}>
                                Cancel
                            </Button>
                        </Grid>
                        <Grid item>
                            <Button variant="outlined" color="primary" onClick={onSubmit}>
                                Submit Package
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Container>
    );
}