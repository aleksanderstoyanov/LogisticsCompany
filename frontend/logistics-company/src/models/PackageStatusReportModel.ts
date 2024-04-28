export class PackageStatusReportModel{
    address: string;
    weight: number;
    toOffice: boolean;
    fromUser: string;
    toUser: string;
    packageStatusName: string

    constructor(address: string, weight: number, toOffice: boolean, fromUser: string, toUser: string, packageStatusName: string){
        this.address = address;
        this.weight = weight;
        this.toOffice = toOffice;
        this.fromUser = fromUser;
        this.toUser = toUser;
        this.packageStatusName = packageStatusName;
    }
}