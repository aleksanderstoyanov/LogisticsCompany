type Nullable<T> = T | undefined | null;

export class DeliveryModel{
    startDate: Date
    endDate: Nullable<Date>

    constructor(startDate: Date, endDate: Date){
        this.startDate = startDate;
        this.endDate = endDate;
    }
}