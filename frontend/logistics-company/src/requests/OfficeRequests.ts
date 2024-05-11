import axios, { AxiosRequestConfig } from "axios";
import { constructRequest } from "../util/Common";
import { HttpRequestType } from "../util/RequestType";


export const getAllOffices = (jwt: string) => {
    var request = constructRequest(
        "Offices/GetAll",
        HttpRequestType.GET,
        jwt,
        true
    ) as AxiosRequestConfig;

    return axios(request)
}


export const createOffice = (jwt: string, data: any) => {
    var request = constructRequest(
        "Offices/Create",
        HttpRequestType.POST,
        jwt,
        true,
        data
    );

    return axios(request);
}

export const updateOffice = (jwt: string, data: any) => {
    var request = constructRequest(
        "Offices/Update",
        HttpRequestType.PUT,
        jwt,
        true,
        data
    );
    
    return axios(request);
}

export const deleteOffice = (id: any, jwt: string) => {
    var request = constructRequest(
        `Offices/Delete?id=${id}`,
        HttpRequestType.DELETE,
        jwt,
        true
    );

    return axios(request);
}