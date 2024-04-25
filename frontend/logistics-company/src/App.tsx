import { Navigation } from './components/Navigation';
import { Home } from './components/Home';
import { Login } from './components/auth/Login';
import { Register } from './components/auth/Register'

import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import '../../logistics-company/src/styles/App.css';
import AdminPanel from './components/admin/AdminPanel';
import OfficePanel from './components/admin/OfficePanel';
import Offices from './components/clients/Offices';
import Packages from './components/employees/Packages';

const darkTheme = createTheme({
  palette: {
    mode: 'dark',
  },
});

function App() {
  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />
      <Navigation></Navigation>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Home/>}></Route>
          <Route path="login" element={<Login />}></Route>
          <Route path="register" element={<Register />}></Route>
          <Route path="adminPanel" element={<AdminPanel />}></Route>
          <Route path="officePanel" element={<OfficePanel />}></Route>
          <Route path="offices" element={<Offices />}></Route>
          <Route path="packages" element={<Packages />}></Route>
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}

export default App;
