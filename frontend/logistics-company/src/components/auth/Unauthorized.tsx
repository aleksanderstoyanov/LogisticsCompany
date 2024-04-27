import { Box, Typography } from "@mui/material";

export default function UnauthorizedPage(){
    return(
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