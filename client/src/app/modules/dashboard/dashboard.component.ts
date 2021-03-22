import { Component, OnInit, ViewChild } from '@angular/core';
import { SalesRecordsService } from './services/sales-records.service';
import { SalesRecord } from '../../core/interfaces/salesRecord';
import { MatDialog } from '@angular/material/dialog';
import { DetailsDialogComponent } from './components/details-dialog/details-dialog.component';
import { AgGridAngular } from 'ag-grid-angular';
import { UploadDialogComponent } from './components/upload-dialog/upload-dialog.component';


@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
    @ViewChild('agGrid') agGrid: AgGridAngular;

    salesRecords: SalesRecord[] = [];

    page = 1;
    hasNext = false;
    totalPages = 0;

    columnDefs = [
        { field: 'region', sortable: true, filter: true, checkboxSelection: true },
        { field: 'country', sortable: true, filter: true },
        { field: 'itemType', sortable: true, filter: true },
        { field: 'salesChannel', sortable: true, filter: true },
        { field: 'orderPriority', sortable: true, filter: true },
        { field: 'orderDate', sortable: true, filter: true },
        { field: 'orderId', sortable: true, filter: true },
        { field: 'shipDate', sortable: true, filter: true },
        { field: 'unitsSold', sortable: true, filter: true },
        { field: 'unitPrice', sortable: true, filter: true },
        { field: 'unitCost', sortable: true, filter: true },
        { field: 'totalRevenue', sortable: true, filter: true },
        { field: 'totalCost', sortable: true, filter: true },
        { field: 'totalProfit', sortable: true, filter: true },
    ];

    constructor(
        private salesRecordsService: SalesRecordsService,
        public dialog: MatDialog,
    ) { }

    async ngOnInit() {
        await this.loadRecords(1);
        this.salesRecordsService.shouldUpdate.subscribe(_ => this.loadRecords(1));
    }

    async loadRecords(page: number) {
        const response = await this.salesRecordsService.getSalesRecords(page);
        this.salesRecords = response.items;
        this.page = response.page;
        this.hasNext = response.hasNext;
        this.totalPages = Math.ceil(response.totalCount / response.pageSize);
    }

    async onNextPageClick() {
        await this.loadRecords(this.page + 1);
    }

    async onPrevPageClick() {
        await this.loadRecords(this.page - 1);
    }

    openModifyDialog(): void {
        const selectedNodes = this.agGrid.api.getSelectedNodes();
        const selectedData = selectedNodes.map(node => node.data);
        if (selectedData[0]) {
            this.dialog.open(DetailsDialogComponent, { width: '500px', data: selectedData[0] });
        }
    }

    openUploadDialog(): void {
        this.dialog.open(UploadDialogComponent, { width: '1000px' });
    }

    async deleteTransaction() {
        const selectedNodes = this.agGrid.api.getSelectedNodes();
        const selectedData = selectedNodes.map(node => node.data);
        if (selectedData.length > 0) {
            selectedData.forEach(i => this.salesRecordsService.deleteSalesRecord(i.id));
        }
    }
}

