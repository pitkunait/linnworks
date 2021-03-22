import { NgModule } from '@angular/core';
import { AppMaterialModule } from './app-material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RoundPipe } from './pipes/round.pipe';
import { EnvPipe } from './pipes/env.pipe';
import { AgGridModule } from 'ag-grid-angular';
import { MatFileUploadModule } from 'angular-material-fileupload';

@NgModule({
    declarations: [
        RoundPipe,
        EnvPipe
    ],
    imports: [
        CommonModule,
        RouterModule,
        AppMaterialModule,
        ReactiveFormsModule,
        FormsModule,
        AgGridModule.withComponents([]),
    ],
    exports: [
        // modules
        CommonModule,
        RouterModule,
        AppMaterialModule,
        ReactiveFormsModule,
        FormsModule,
        AgGridModule,
        MatFileUploadModule,

        // pipes
        RoundPipe,
        EnvPipe
    ],
})
export class SharedModule {}
