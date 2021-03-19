import { NgModule } from '@angular/core';
import { MomentPipe } from './pipes/moment.pipe';
import { AppMaterialModule } from './app-material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RoundPipe } from './pipes/round.pipe';
import { ChartsModule } from 'ng2-charts';


@NgModule({
    declarations: [
        MomentPipe,
        RoundPipe
    ],
    imports: [
        CommonModule,
        RouterModule,
        AppMaterialModule,
        ReactiveFormsModule,
        FormsModule,
        ChartsModule
    ],
    exports: [
        // modules
        CommonModule,
        RouterModule,
        AppMaterialModule,
        ReactiveFormsModule,
        FormsModule,
        ChartsModule,

        // pipes
        MomentPipe,
        RoundPipe
    ],
})
export class SharedModule {}
