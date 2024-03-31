import { AppBar, Box, Button, Icon, IconButton, Link, Toolbar, Typography } from "@mui/material";
import LocalShippingIcon from '@mui/icons-material/LocalShipping';

export function Navigation() {
    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static">
                <Toolbar>
                    <Typography variant="h6" sx={{ flexGrow: 1 }} style={{ marginTop: "5"}}>
                        <Link underline="none" href="/">
                            Logistics Company
                        </Link>
                    </Typography>
                    <Typography variant="h6" component="div">
                    <Link variant="h6" href="login" underline="none">
                            <Button color="inherit">
                                Login
                            </Button>
                        </Link>
                        <Link variant="h6" href="register" underline="none">
                            <Button color="inherit">
                                Register
                            </Button>
                        </Link>
                    </Typography>
                </Toolbar>
            </AppBar>
        </Box>
    );
}