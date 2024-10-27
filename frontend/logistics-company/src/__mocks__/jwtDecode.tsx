import { jwtDecode } from "jwt-decode";

export default function mockJwtDecode(role: string) {
    return (jwtDecode as jest.Mock).mockReturnValue({
        Email: "test@gmail.com",
        Role: role
    });
}