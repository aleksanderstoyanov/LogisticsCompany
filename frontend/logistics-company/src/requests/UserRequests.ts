import axios from "axios"
import { constructRequest } from "../util/Common"
import { HttpRequestType } from "../util/RequestType"

export const getAllUsers = (jwt: string) => {
    const request = constructRequest(
        "Users/GetAll",
        HttpRequestType.GET,
        jwt,
        true
    );

    return axios(request);
}

export const getAllUsersExcept = (jwt: string, id: any, role: string) => {
    const request = constructRequest(
        `Users/GetAllExcept?id=${id}&role=${role}`,
        HttpRequestType.GET,
        jwt,
        true,
    )

    return axios(request);
}

export const updateUser = (jwt: string, data: any) => {
    const request = constructRequest(
        "Users/Update",
        HttpRequestType.PUT,
        jwt,
        true,
        data
    );

    return axios(request);
}

export const deleteUser = (id: any, jwt: string) => {
    const request = constructRequest(
        `Users/Delete?id=${id}`,
        HttpRequestType.DELETE,
        jwt,
        true
    );

    return axios(request);
}