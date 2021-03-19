import { Component, OnInit } from '@angular/core';
import { TransactionService } from './services/transaction.service';
import { Transaction } from '../../core/interfaces/transaction.interface';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from './dialog/dialog.component';


@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {

    displayedColumns: string[] = [
        'asset', 'amountCash', 'amountCrypto', 'date',
        'current', 'profitloss', 'delete', 'graph',
    ];
    transactions: Transaction[] = [];
    totalInvested = 0;
    totalCurrentValue = 0;
    lossProfit = 0;

    constructor(private transactionService: TransactionService, public dialog: MatDialog) { }

    async ngOnInit() {
        await this.loadTransactions();
        this.transactionService.shouldUpdate.subscribe(e => this.loadTransactions());
    }

    async loadTransactions() {
        this.transactions = await this.transactionService.loadTransactions();
        this.totalInvested = 0;
        this.totalCurrentValue = 0;
        this.lossProfit = 0;
        this.transactions.forEach((i) => {
                this.totalInvested += i.amountCash;
                this.totalCurrentValue += i.currentValue;
                this.lossProfit += i.currentValue - i.amountCash;
            },
        );
    }

    openAddDialog(): void {
        this.dialog.open(DialogComponent, { width: '500px' });
    }

    async deleteTransaction(id: number) {
        await this.transactionService.deleteTransaction(id);
    }
}

