import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ApiInterceptor } from './interceptors/api.interceptor';


@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        RouterModule,
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
    ],
})
export class CoreModule {}
