import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { DashboardComponent } from './dashboard.component';
import { RouterModule } from '@angular/router';
import { dashboardRoutes } from './dashboard.routes';
import { ChartComponent } from './chart/chart.component';
import { DialogComponent } from './dialog/dialog.component';


@NgModule({
    declarations: [
        DashboardComponent,
        ChartComponent,
        DialogComponent,
    ],
    imports: [
        RouterModule.forChild(dashboardRoutes),
        SharedModule,
    ],
    bootstrap: [DashboardComponent]
})
export class DashboardModule {}

