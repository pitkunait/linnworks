import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { TokenService } from '../services/token.service';
import { catchError, filter, finalize, switchMap, take } from 'rxjs/operators';
import { environment } from '../../../environments/environment';


@Injectable({
    providedIn: 'root',
})
export class ApiInterceptor implements HttpInterceptor {
    private AUTH_HEADER = 'Authorization';
    private refreshTokenInProgress = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    constructor(private tokenService: TokenService, private httpClient: HttpClient) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): any {

        if (!req.headers.has('Content-Type')) {
            req = req.clone({
                headers: req.headers.set('Content-Type', 'application/json'),
            });
        }

        req = this.addAuthenticationToken(req);

        return next
            .handle(req)
            .pipe(
                catchError((error: HttpErrorResponse) => {
                    if (error && error.status === 401) {
                        if (this.refreshTokenInProgress) {
                            return this.refreshTokenSubject.pipe(
                                filter(result => result !== null),
                                take(1),
                                switchMap(() => next.handle(this.addAuthenticationToken(req))),
                            );
                        } else {
                            this.refreshTokenInProgress = true;
                            this.refreshTokenSubject.next(null);
                            return this.refreshAccessToken().pipe(
                                switchMap((res: any) => {
                                    this.tokenService.setAccessToken(res.access);
                                    this.refreshTokenSubject.next(res);
                                    return next.handle(this.addAuthenticationToken(req));
                                }),
                                finalize(() => this.refreshTokenInProgress = false),
                            );
                        }
                    } else {
                        return throwError(error);
                    }
                }),
            );
    }

    private refreshAccessToken(): Observable<any> {
        return this.httpClient
            .post(`${environment.baseUrl}/auth/refresh/`, { refresh: this.tokenService.getRefreshToken() });
    }

    private addAuthenticationToken(request: HttpRequest<any>): HttpRequest<any> {
        if (!this.tokenService.hasToken()) {
            return request;
        }
        if (request.url.match(environment.baseUrl)) {
            // console.log(`Setting token ${this.tokenService.getAccessToken()}`);
            return request.clone({
                headers: request.headers
                    .append(this.AUTH_HEADER, 'Bearer ' + this.tokenService.getAccessToken())
            });
        }


        return request;
    }

}
