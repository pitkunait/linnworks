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
}


@Injectable({ providedIn: 'root' })
export class SalesRecordsService {
    private url = 'sales';

    constructor(private http: HttpClient) { }

    @Output() shouldUpdate = new EventEmitter();

    async getSalesRecords(page: number = 1): Promise<SalesRecordsResponse> {
        const records = await this.http.get(`${environment.baseUrl}/sales`, { params: { page: page.toString() } }).toPromise();
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
        return this.http.post('http://localhost:5000/api/sales/upload', formData, { reportProgress: true, observe: 'events' });
    }

    async importFile(fileName) {
        await this.http.get('http://localhost:5000/api/sales/import', {params: {path: fileName}}).toPromise();
        this.shouldUpdate.emit();
    }

    async deleteSalesRecord(id: number) {
        await this.http.delete(`${environment.baseUrl}/sales/${id}`).toPromise();
        this.shouldUpdate.emit();
    }

    async editSalesRecord(salesRecord: SalesRecord) {
        await this.http.put(`${environment.baseUrl}/sales/${salesRecord.id}`, salesRecord).toPromise();
        this.shouldUpdate.emit();
    }
}
