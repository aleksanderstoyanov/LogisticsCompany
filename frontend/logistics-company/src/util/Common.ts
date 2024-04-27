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