import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SalesRecordsService } from '../../services/sales-records.service';
import { SalesRecord } from '../../../../core/interfaces/salesRecord';


@Component({
    selector: 'app-dialog',
    templateUrl: './details-dialog.component.html',
    styleUrls: ['./details-dialog.component.scss'],
})
export class DetailsDialogComponent implements OnInit {
    form: FormGroup;

    fields = [
        { name: 'region', title: 'Region' },
        { name: 'country', title: 'Country' },
        { name: 'itemType', title: 'Item Type' },
        { name: 'salesChannel', title: 'Sales Channel' },
        { name: 'orderPriority', title: 'Order Priority' },
        { name: 'orderDate', title: 'Order Date' },
        { name: 'orderId', title: 'Order ID' },
        { name: 'shipDate', title: 'Ship Date' },
        { name: 'unitsSold', title: 'Units Sold' },
        { name: 'unitPrice', title: 'Unit Price' },
        { name: 'unitCost', title: 'Unit Cost' },
        { name: 'totalRevenue', title: 'Total Revenue' },
        { name: 'totalCost', title: 'Total Cost' },
        { name: 'totalProfit', title: 'Total Profit' },
    ];

    constructor(
        private formBuilder: FormBuilder,
        private dialogRef: MatDialogRef<DetailsDialogComponent>,
        private salesRecordsService: SalesRecordsService,
        @Inject(MAT_DIALOG_DATA) public data: SalesRecord,
    ) {
    }

    ngOnInit() {
        this.form = this.formBuilder.group(this.fields.reduce((acc, item) => {
            acc[item.name] = [this.data[item.name], Validators.required];
            return acc;
        }, {}));
    }

    isFieldInvalid(field: string) {
        return (
            (!this.form.get(field).valid && this.form.get(field).touched) ||
            (this.form.get(field).untouched)
        );
    }


    async onModify() {
        if (this.form.valid) {
            const data = {...this.data, ...this.form.value} as SalesRecord;
            await this.salesRecordsService.editSalesRecord(data);
            this.dialogRef.close();
        }
    }

}
