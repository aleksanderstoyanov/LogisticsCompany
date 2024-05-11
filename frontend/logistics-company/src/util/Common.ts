import { API_URL } from "./Constants";
import { HttpRequestType } from "./RequestType";

export function deepEqual(x: any, y: any): boolean {
    return (x && y && typeof x === 'object' && typeof y === 'object') ?
        (Object.keys(x).length === Object.keys(y).length) &&
        Object.keys(x).reduce(function (isEqual: boolean, key: any) {
            return isEqual && deepEqual(x[key], y[key]);
        }, true) : (x === y);
}

export function valueOptionMapper(entity: any, dataValueField: string, dataTextField: string) {
    return {
        value: entity[dataValueField],
        label: entity[dataTextField]
    }
}

export function getRandomInt(min: number, max: number) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

export function constructRequest(url: string, method: HttpRequestType, jwt: string, authorize: boolean, data?: any,) {
    const requestMethod = method.toString();

    switch (method) {
        case HttpRequestType.GET:
        case HttpRequestType.DELETE:
            var request = {
                method: requestMethod,
                url: `${API_URL}/${url}`
            } as any;

            if (authorize != null) 
                request["headers"] = { "Authorization": `Bearer ${jwt}` };

            return request;
        case HttpRequestType.POST:
        case HttpRequestType.PUT:
            var request = {
                method: requestMethod,
                url: `${API_URL}/${url}`,
                data: data
            } as any;

            if (authorize)
             request["headers"] = { "Authorization": `Bearer ${jwt}` };

            return request;
        default:
            break;
    }
}