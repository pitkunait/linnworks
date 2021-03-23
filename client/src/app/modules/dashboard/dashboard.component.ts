import { Component, OnInit, ViewChild } from '@angular/core';
import { SalesRecordsService } from './services/sales-records.service';
import { SalesRecord } from '../../core/interfaces/salesRecord';
import { MatDialog } from '@angular/material/dialog';
import { DetailsDialogComponent } from './components/details-dialog/details-dialog.component';
import { AgGridAngular } from 'ag-grid-angular';
import { UploadDialogComponent } from './components/upload-dialog/upload-dialog.component';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { TokenService } from '../../core/services/token.service';


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
    totalCount = 0;
    totalProfit = 0;
    countries = [];
    columnDefs = [
        { field: 'region', checkboxSelection: true },
        { field: 'country' },
        { field: 'itemType' },
        { field: 'salesChannel' },
        { field: 'orderPriority' },
        { field: 'orderDate', sortable: true },
        { field: 'orderId' },
        { field: 'shipDate' },
        { field: 'unitsSold' },
        { field: 'unitPrice' },
        { field: 'unitCost' },
        { field: 'totalRevenue' },
        { field: 'totalCost' },
        { field: 'totalProfit' },
    ];


    constructor(
        public salesRecordsService: SalesRecordsService,
        public dialog: MatDialog
    ) { }

    async ngOnInit() {
        await this.loadRecords(1);
        this.salesRecordsService.shouldUpdate.subscribe(_ => this.loadRecords(1));
        this.agGrid.sortChanged.subscribe(_ => {
            const sort = this.agGrid.api.getSortModel()[0];
            if (sort) {
                this.salesRecordsService.sortBy = sort.colId;
                this.salesRecordsService.direction = sort.sort;
            } else {
                this.salesRecordsService.sortBy = 'id';
                this.salesRecordsService.direction = 'asc';
            }
            this.loadRecords(this.page);
        });
        this.countries = await this.salesRecordsService.getCountries();
    }

    onCountryChange(country: string) {
        this.salesRecordsService.country = country;
    }

    onYearChange(event: Event) {
        this.salesRecordsService.year = (event.target as HTMLInputElement).value;
    }

    async loadRecords(page: number) {
        const response = await this.salesRecordsService.getSalesRecords(page);
        this.salesRecords = response.items;
        this.page = response.page;
        this.hasNext = response.hasNext;
        this.totalProfit = response.totalProfit;
        this.totalCount = response.totalCount;
        this.totalPages = Math.ceil(response.totalCount / response.pageSize);
    }

    async onNextPageClick() {
        if (!this.salesRecordsService.isBusy) {
            await this.loadRecords(this.page + 1);
        }

    }

    async onPrevPageClick() {
        if (!this.salesRecordsService.isBusy) {
            await this.loadRecords(this.page - 1);
        }
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
        await this.salesRecordsService.deleteSalesRecord(selectedData.map(i => String(i.id)));
    }
}

