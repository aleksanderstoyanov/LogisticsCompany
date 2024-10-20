
import { jwtDecode } from 'jwt-decode';
import { render, screen } from '@testing-library/react';
import { Home } from '../Home';

jest.mock("jwt-decode");

test('Home Page is rendered with anonymous message if not authenticated', () => {
  render(<Home />);
  const linkElement = screen.getByText(/Welcome Anonymous, to Logistics Company!/i);
  expect(linkElement).toBeInTheDocument();
});

test('Home Page is rendered with username if authenticated', () => {

  (jwtDecode as jest.Mock).mockReturnValue({
      Email: "test@gmail.com",
      Role: "Admin"
  });

  sessionStorage["jwt"] = "Test";
  render(<Home />)
  
  const linkElement = screen.getByText(/Welcome test@gmail.com, to Logistics Company!/i)
  expect(linkElement).toBeInTheDocument();
  
})
