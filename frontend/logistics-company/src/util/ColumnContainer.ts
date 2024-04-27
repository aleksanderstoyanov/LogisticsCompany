import { GridColDef, ValueOptions } from "@mui/x-data-grid";

export class ColumnContainer {
    private columns: GridColDef[]

    constructor() {
        this.columns = [];
    }

    Add(field: string,
        headerName: string,
        width: number,
        editable: boolean,
        type: any = "string",
        valueOptions?: ValueOptions[]
    ): ColumnContainer {
        
        let column = {
            field: field,
            headerName: headerName,
            width: width,
            editable: editable,
            type: type,
            valueOptions: valueOptions
        } as GridColDef;

        this.columns.push(column);
        return this;
    }

    GetColumns(): GridColDef[] {
        return this.columns;
    }
}