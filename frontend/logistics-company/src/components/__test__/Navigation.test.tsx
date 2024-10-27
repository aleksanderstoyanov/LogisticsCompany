import { render, screen } from "@testing-library/react";
import { Navigation } from "../Navigation"
import mockJwtDecode from "../../__mocks__/jwtDecode";

jest.mock("jwt-decode");

test("Navigation should render correct items for anonymous user", () => {
    render(<Navigation />);

    const linkElement = screen.getByText(/Register/i);
    const heading = screen.getByText(/Logistics Company/i);

    expect(linkElement).toBeInTheDocument();
    expect(heading).toBeInTheDocument();
})

test("Navigation should render correct items for Admin", () => {
    mockJwtDecode("Admin");

    const LINKS = ['Admin Panel', 'Office Panel', 'Reports', 'Logout'];

    sessionStorage["jwt"] = "Test";

    const { container } = render(<Navigation />);
    const links = container.querySelectorAll("a[style*='display: inline']")

    expect(links.length).toEqual(4);
    links.forEach((element: Element) => {
        const button = element.getElementsByTagName("button")[0] as HTMLButtonElement;
        let text = button.childNodes[0].textContent as string;
        expect(LINKS.includes(text)).toBe(true);
    })
})

test("Navigation should render correct items for Client", () => {
    mockJwtDecode("Client");

    const LINKS = ['Offices', 'Sent Packages', 'Received Packages' , 'Logout'];

    sessionStorage["jwt"] = "Test";

    const { container } = render(<Navigation />);
    const links = container.querySelectorAll("a[style*='display: inline']")

    expect(links.length).toEqual(4);
    links.forEach((element: Element) => {
        const button = element.getElementsByTagName("button")[0] as HTMLButtonElement;
        let text = button.childNodes[0].textContent as string;
        expect(LINKS.includes(text)).toBe(true);
    })
})

test("Navigation should render correct items for Office Employee", () => {
    mockJwtDecode("OfficeEmployee");

    const LINKS = ['Packages', 'Logout'];

    sessionStorage["jwt"] = "Test";

    const { container } = render(<Navigation />);
    const links = container.querySelectorAll("a[style*='display: inline']")

    expect(links.length).toEqual(2);
    links.forEach((element: Element) => {
        const button = element.getElementsByTagName("button")[0] as HTMLButtonElement;
        let text = button.childNodes[0].textContent as string;
        expect(LINKS.includes(text)).toBe(true);
    })
})

test("Navigation should render correct items for Courier", () => {
    mockJwtDecode("Courier");

    const LINKS = ['Packages', 'Deliveries', 'Logout'];

    sessionStorage["jwt"] = "Test";

    const { container } = render(<Navigation />);
    const links = container.querySelectorAll("a[style*='display: inline']")

    expect(links.length).toEqual(3);
    links.forEach((element: Element) => {
        const button = element.getElementsByTagName("button")[0] as HTMLButtonElement;
        let text = button.childNodes[0].textContent as string;
        expect(LINKS.includes(text)).toBe(true);
    })
})