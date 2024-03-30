import React from 'react';
import { Register } from './components/Register'
import logo from './logo.svg';
import './App.css';

import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

const darkTheme = createTheme({
  palette: {
    mode: 'dark',
  },
});

function App() {
  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />
      <Register></Register>
    </ThemeProvider>
  );
}

export default App;
