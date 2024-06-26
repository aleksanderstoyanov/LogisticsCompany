import { Navigation } from './components/Navigation';
import { Home } from './components/Home';
import { Login } from './components/auth/Login';
import { Register } from './components/auth/Register'

import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import '../../logistics-company/src/styles/App.css';
import AdminPanel from './components/admin/panels/AdminPanel';
import OfficePanel from './components/admin/panels/OfficePanel';
import Offices from './components/clients/Offices';
import Packages from './components/employees/Packages';
import ReceivedPackagesPanel from './components/clients/ReceivedPackagesPanel';
import SentPackagesPanel from './components/clients/SentPackagesPanel';
import Reports from './components/admin/reports/Reports';
import Deliveries from './components/employees/Deliveries';

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
          <Route path="reports" element={<Reports />}></Route>
          <Route path="packages" element={<Packages />}></Route>
          <Route path="sentPackages" element={<SentPackagesPanel />}></Route>
          <Route path="receivedPackages" element={<ReceivedPackagesPanel />}></Route>
          <Route path="deliveries" element={<Deliveries />}></Route>
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}

export default App;
