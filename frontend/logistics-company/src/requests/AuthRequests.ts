import axios from "axios";
import { HttpRequestType } from "../util/RequestType";
import { constructRequest } from "../util/Common";
import { LoginModel } from "../models/LoginModel";
import { RegisterModel } from "../models/RegisterModel";

export const login = (data: LoginModel) => {
    var request = constructRequest(
        "Authorization/Login",
        HttpRequestType.POST,
        "",
        false,
        data
    );

    return axios(request);
}

export const register = (data: RegisterModel) => {
    var request = constructRequest(
        "Authorization/Register",
        HttpRequestType.POST,
        "",
        false,
        data
    );

    return axios(request);
}
