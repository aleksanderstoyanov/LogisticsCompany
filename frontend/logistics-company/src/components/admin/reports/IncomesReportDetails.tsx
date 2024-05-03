import { Alert, Box, Button, Grid, Typography } from "@mui/material";
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import PointOfSaleIcon from '@mui/icons-material/PointOfSale';
import { SyntheticEvent, useState } from "react";
import { API_URL, END_PERIOD_ID, START_PERIOD_ID } from "../../../util/Constants";
import dayjs from "dayjs";
import axios from "axios";

export default function IncomesReportDetails() {
    const jwt = sessionStorage["jwt"];

    const [income, setIncome] = useState<number>(0.0);
    const [incomeMessageVisible, setIncomeVisible] = useState<boolean>(false);

    const [startPeriod, setStartPeriod] = useState<Date | undefined>(undefined);
    const [endPeriod, setEndPeriod] = useState<Date | undefined>(undefined);


    const [alertVisible, setAlertVisible] = useState<boolean>(false);
    const [alertMessage, setAlertMessage] = useState<string>("");

    function onClick() {
        if (startPeriod?.toString() == "Invalid Date" || endPeriod?.toString() == "Invalid Date") {
            setAlertVisible(true);
            setAlertMessage("Start or End Period is with an invalid date!");
        }

        else if (!startPeriod || !endPeriod) {
            setAlertVisible(true);
            setAlertMessage("Start or End Period is not filled!");
        }

        else if(startPeriod && endPeriod && startPeriod > endPeriod){
            setAlertVisible(true);
            setAlertMessage("Start Date must not be after End Period!");
        }

        else {
            setAlertVisible(false);
            axios({
                method:"GET",
                url: `${API_URL}/Reports/GetIncomesForPeriod?startPeriod=${startPeriod.toDateString()}&endPeriod=${endPeriod.toDateString()}`,
                headers: {
                    "Authorization": `Bearer ${jwt}`
                }
            })
            .then((response) => {
                const income = response.data.income;
                setIncomeVisible(true);
                setIncome(income);
            })
            
        }
    }
    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>

            <Box sx={{
                display: "flex",
                justifyContent: "center"
            }}>
                <Typography variant="h6" align="center" >
                    Select Period
                </Typography>
            </Box>
            <Grid display={"flex"} justifyContent={"center"} alignItems={"center"}>
                <DatePicker
                    name={START_PERIOD_ID}
                    label="StartPeriod"
                    onChange={(newValue) => setStartPeriod(newValue?.toDate())}
                    sx={{
                        margin: "5%"
                    }} />
                <DatePicker name={END_PERIOD_ID}
                    label="EndPeriod"
                    onChange={(newValue) => setEndPeriod(newValue?.toDate())}
                    sx={{
                        margin: "5%"
                    }} />
            </Grid>
            <Box display={"flex"} justifyContent={"center"}>
                <Button variant="outlined"
                    endIcon={<PointOfSaleIcon />}
                    onClick={onClick}>
                    Total Incomes for Period
                </Button>
            </Box>
            <Box display={"flex"} justifyContent={"center"} marginTop={"2.5em"}>
                <Typography variant="h6" sx={{
                    display: incomeMessageVisible ? "block": "none"
                }}>
                    Income from <strong>{startPeriod?.toDateString()} </strong> 
                           to <strong>{endPeriod?.toDateString()} </strong>
                           is ${income}
                </Typography>
            </Box>
            <Alert severity="error"
                sx={{
                    display: alertVisible ? "block" : "none"
                }}>
                {alertMessage}
            </Alert>
        </LocalizationProvider>
    )
}