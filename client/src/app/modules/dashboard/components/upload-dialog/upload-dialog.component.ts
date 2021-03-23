import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import Papa from 'papaparse';
import { camelCase } from 'lodash';
import { SalesRecordsService } from '../../services/sales-records.service';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { SalesRecord } from '../../../../core/interfaces/salesRecord';


@Component({
    selector: 'app-dialog',
    templateUrl: './upload-dialog.component.html',
    styleUrls: ['./upload-dialog.component.scss'],
})
export class UploadDialogComponent implements OnInit {

    private selectedFile: File;
    public preview: SalesRecord[] = [];
    public progress: number;

    @Output() public uploadFinished = new EventEmitter();

    columnDefs = [
        { field: 'region' },
        { field: 'country' },
        { field: 'itemType' },
        { field: 'salesChannel' },
        { field: 'orderPriority' },
        { field: 'orderDate' },
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
        private dialogRef: MatDialogRef<UploadDialogComponent>,
        private http: HttpClient,
        public salesRecordsService: SalesRecordsService,
    ) {}

    ngOnInit() {

    }

    public uploadFile = async () => {
        const formData = new FormData();
        formData.append('file', this.selectedFile, this.selectedFile.name);
        this.http.post('http://localhost:5000/api/sales/upload', formData, { reportProgress: true, observe: 'events' })
            .subscribe(event => {
                if (event.type === HttpEventType.UploadProgress) {
                    this.progress = Math.round(100 * event.loaded / event.total);
                } else if (event.type === HttpEventType.Response) {
                    this.uploadFinished.emit(event.body);
                    const body: any = event.body;
                    this.salesRecordsService.importFile(body.dbPath);
                    this.dialogRef.close();
                }
            });
    };


    async onFileSelect(files) {
        if (files && files.length) {
            this.selectedFile = files[0];
            this.preview = await this.previewCSV(this.selectedFile);
        }
    }

    async previewCSV(file): Promise<SalesRecord[]> {
        return new Promise((resolve, reject) => {
            Papa.parse(file,
                {
                    header: true,
                    dynamicTyping: true,
                    skipEmptyLines: true,
                    preview: 20,
                    transformHeader: camelCase,
                    complete: (results) => resolve(results.data),
                    error: () => reject('Parsing Error'),
                },
            );
        });
    }

}
