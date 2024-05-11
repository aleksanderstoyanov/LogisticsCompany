import axios from "axios";
import PackageModel from "../models/PackageModel";
import { constructRequest } from "../util/Common";
import { HttpRequestType } from "../util/RequestType";


export const getAllPackages = (jwt: string) => {
    const request = constructRequest(
        "Packages/GetAll",
        HttpRequestType.GET,
        jwt,
        true
    );

    return axios(request);
}

export const getReceivedPackages = (jwt: string, id: any) => {
    const request = constructRequest(
        `Packages/GetReceived?id=${id}`,
        HttpRequestType.GET,
        jwt,
        true
    )

    return axios(request);
}

export const getSentPackages = (jwt: string, id: any) => {
    const request = constructRequest(
        `Packages/GetSent?id=${id}`,
        HttpRequestType.GET,
        jwt,
        true
    );

    return axios(request);
}

export const createPackage = (jwt: string, data: PackageModel) => {
    const request = constructRequest(
        "Packages/Create",
        HttpRequestType.POST,
        jwt,
        true,
        data
    );

    return axios(request);
}

export const updatePackage = (jwt: string, data: any) => {
    const request = constructRequest(
        "Packages/Update",
        HttpRequestType.PUT,
        jwt,
        true,
        data
    );

    return axios(request);
}

export const deletePackage = (id: any, jwt: string) => {
    const request = constructRequest(
        `Packages/Delete?id=${id}`,
        HttpRequestType.DELETE,
        jwt,
        true
    );

    return axios(request);
}