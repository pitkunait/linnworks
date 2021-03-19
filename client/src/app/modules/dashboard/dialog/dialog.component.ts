import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TransactionService } from '../services/transaction.service';


@Component({
    selector: 'app-dialog',
    templateUrl: './dialog.component.html',
    styleUrls: ['./dialog.component.scss'],
})
export class DialogComponent implements OnInit {
    form: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private dialogRef: MatDialogRef<DialogComponent>,
        private transactionService: TransactionService,
    ) {}

    ngOnInit() {
        this.form = this.formBuilder.group({
            asset: ['', Validators.required],
            amountCash: ['', Validators.required],
            currency: ['', Validators.required],
            amountCrypto: [''],
            date: ['', Validators.required],
        });
    }

    isFieldInvalid(field: string) {
        return (
            (!this.form.get(field).valid && this.form.get(field).touched) ||
            (this.form.get(field).untouched)
        );
    }


    async onSubmit() {
        if (this.form.valid) {
            await this.transactionService.createTransaction(this.form.value);
            this.dialogRef.close();
        }
    }

}
