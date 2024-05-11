import axios from "axios";
import { constructRequest } from "../util/Common";
import { HttpRequestType } from "../util/RequestType"

export const getAllClients = (jwt: string) => {
    const request = constructRequest(
        "Reports/AllClients",
        HttpRequestType.GET,
        jwt,
        true
    );

    return axios(request);
}

export const getIncomesForPeriod = (jwt: string, startPeriod: string, endPeriod: string) => {
    const request = constructRequest(
        `Reports/GetIncomesForPeriod?startPeriod=${startPeriod}&endPeriod=${endPeriod}`,
        HttpRequestType.GET,
        jwt,
        true
    );

    return axios(request);
}