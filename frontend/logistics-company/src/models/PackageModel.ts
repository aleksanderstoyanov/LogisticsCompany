export default class PackageModel {
    address: string
    from: number
    to: number
    weight: number
    toOffice: boolean

    constructor(address: string, from: number, to: number, weight: number, toOffice: boolean) {
        this.address = address;
        this.from = from;
        this.to = to;
        this.weight = weight;
        this.toOffice = toOffice;
    }
}