export default class PackageModel {
    address: string
    fromId: number
    toId: number
    officeId: number
    weight: number
    toOffice: boolean

    constructor(address: string, fromId: number, toId: number, weight: number, toOffice: boolean, officeId: number) {
        this.address = address;
        this.fromId = fromId;
        this.toId = toId;
        this.weight = weight;
        this.toOffice = toOffice;
        this.officeId = officeId;
    }
}