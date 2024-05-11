import axios from "axios";
import { constructRequest } from "../util/Common"
import { HttpRequestType } from "../util/RequestType"

export const getAllDeliveries = (jwt: string) => {
    const request = constructRequest(
        "Deliveries/GetAll",
        HttpRequestType.GET,
        jwt,
        true
    );

    return axios(request);
}

export const createDelivery = (jwt: string, data: any) => {
    const request = constructRequest(
        "Deliveries/Create",
        HttpRequestType.POST,
        jwt,
        true,
        data
    )

    return axios(request);
}

export const updateDelivery = (jwt: string, data: any) => {
    const request = constructRequest(
        "Deliveries/Update",
        HttpRequestType.PUT,
        jwt,
        true,
        data
    )

    return axios(request);
}
