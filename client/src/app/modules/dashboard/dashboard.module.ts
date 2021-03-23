import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { DashboardComponent } from './dashboard.component';
import { RouterModule } from '@angular/router';
import { dashboardRoutes } from './dashboard.routes';
import { DetailsDialogComponent } from './components/details-dialog/details-dialog.component';
import { UploadDialogComponent } from './components/upload-dialog/upload-dialog.component';


@NgModule({
    declarations: [
        DashboardComponent,
        DetailsDialogComponent,
        UploadDialogComponent,
    ],
    imports: [
        RouterModule.forChild(dashboardRoutes),
        SharedModule,
    ],
    bootstrap: [DashboardComponent],
})
export class DashboardModule {}

