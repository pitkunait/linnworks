import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { catchError } from 'rxjs/operators';


@Injectable({
    providedIn: 'root',
})
export class ApiService {

    constructor(
        private http: HttpClient,
    ) {}

    formatErrors(error: any) {
        return throwError(error.error);
    }

    get(path: string, params: {
        [param: string]: string | string[] ;
    } = {}): Observable<any> {
        return this.http.get(`${environment.baseUrl}${path}`, { params })
            .pipe(catchError(this.formatErrors));
    }

    put(path: string, body: object = {}): Observable<any> {
        return this.http.put(
            `${environment.baseUrl}${path}`,
            JSON.stringify(body),
        ).pipe(catchError(this.formatErrors));
    }

    post(path: string, body: object = {}): Observable<any> {
        return this.http.post(
            `${environment.baseUrl}${path}`,
            JSON.stringify(body),
        ).pipe(catchError(this.formatErrors));
    }

    delete(path): Observable<any> {
        return this.http.delete(
            `${environment.baseUrl}${path}`,
        ).pipe(catchError(this.formatErrors));
    }
}
