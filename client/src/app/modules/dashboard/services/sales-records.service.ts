import { EventEmitter, Injectable, Output } from '@angular/core';
import { SalesRecord } from '../../../core/interfaces/salesRecord';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';


interface SalesRecordsResponse {
    items: SalesRecord[];
    hasNext: boolean;
    page: number;
    pageSize: number;
    totalCount: number;
    totalProfit: number;
}


@Injectable({ providedIn: 'root' })
export class SalesRecordsService {
    private url = 'sales';
    public sortBy = 'id';
    public direction = 'asc';
    public country = '';
    public year = '';

    constructor(private http: HttpClient) { }

    @Output() shouldUpdate = new EventEmitter();
    public isBusy = false;

    async getSalesRecords(page: number = 1): Promise<SalesRecordsResponse> {
        this.isBusy = true;
        const records = await this.http.get(`${environment.baseUrl}/sales`, {
            params: {
                page: page.toString(),
                sortBy: this.sortBy,
                direction: this.direction,
                country: this.country,
                year: this.year,
            },
        }).toPromise();
        this.isBusy = false;
        return records as SalesRecordsResponse;
    }

    async insertSalesRecords(salesRecords: SalesRecord[]) {
        salesRecords.forEach(i => {
            i.shipDate = new Date(i.shipDate).toISOString();
            i.orderDate = new Date(i.orderDate).toISOString();
        });
        await this.http.post(this.url, salesRecords).toPromise();
        this.shouldUpdate.emit();
    }

    uploadFile(file) {
        const formData = new FormData();
        formData.append('file', file, file.name);
        return this.http.post(`${environment.baseUrl}/sales/upload`, formData, {
            reportProgress: true,
            observe: 'events',
        });
    }

    async importFile(fileName) {
        await this.http.get(`${environment.baseUrl}/sales/import`, { params: { path: fileName } }).toPromise();
        this.shouldUpdate.emit();
    }

    async getCountries() {
        return await this.http.get(`${environment.baseUrl}/sales/countries`).toPromise() as string[];
    }

    async deleteSalesRecord(id: string[]) {
        await this.http.delete(`${environment.baseUrl}/sales`, { params: { id } }).toPromise();
        this.shouldUpdate.emit();
    }

    async editSalesRecord(salesRecord: SalesRecord) {
        await this.http.put(`${environment.baseUrl}/sales/${salesRecord.id}`, salesRecord).toPromise();
        this.shouldUpdate.emit();
    }
}
