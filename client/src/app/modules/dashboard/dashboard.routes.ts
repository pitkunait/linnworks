import { DashboardComponent } from './dashboard.component';
import { Routes } from '@angular/router';


export const dashboardRoutes: Routes = [
    {
        path: '',
        component: DashboardComponent,
        pathMatch: 'full',
    },
];
