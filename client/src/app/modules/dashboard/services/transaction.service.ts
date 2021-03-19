import { Injectable, EventEmitter, Output } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Transaction } from '../../../core/interfaces/transaction.interface';


@Injectable({ providedIn: 'root' })
export class TransactionService {
    private url = 'transactions/';

    constructor(private apiService: ApiService) { }

    @Output() shouldUpdate = new EventEmitter();

    async loadTransactions(): Promise<Transaction[]> {
        const transactions = await this.apiService.get(this.url).toPromise();
        const uniqueValues = new Set<string>(transactions.map(i => i.asset));
        const assets = await Promise.all([...uniqueValues].map(i => this.getAssetData(i)));
        return this.transformTransactions(transactions, assets);
    }

    async createTransaction(values: any) {
        await this.apiService.post(this.url, {
            asset: values.asset,
            amount_cash: values.amountCash,
            amount_crypto: values.amountCrypto || undefined,
            date_of_transfer: new Date(values.date).toISOString(),
            currency: values.currency,
        }).toPromise();
        this.shouldUpdate.emit();
    }

    async deleteTransaction(id: number) {
        await this.apiService.delete(`${this.url}${id}/`).toPromise();
        this.shouldUpdate.emit();
    }

    async getAssetData(asset: string) {
        return await this.apiService.get('yahoo/', { asset }).toPromise();
    }

    private transformTransactions(transactions, assets): Transaction[] {
        return transactions.map(transaction => {
            const amountCrypto = Number(transaction.amount_crypto);
            const amountCash = Number(transaction.amount_cash);
            const asset = assets.find(i => transaction.asset === i.chart.result[0].meta.symbol);
            const currentValue = asset.chart.result[0].meta.regularMarketPrice * amountCrypto;
            const timeSeries = asset.chart.result[0].indicators.quote[0].close
                .filter(i => i)
                .map(i => (i * amountCrypto) - amountCash);
            return {
                amountCash,
                currentValue,
                timeSeries,
                amountCrypto,
                id: transaction.id,
                asset: transaction.asset,
                profitLoss: currentValue - amountCash,
                timeStamp: asset.chart.result[0].timestamp,
                dateOfTransfer: transaction.date_of_transfer,
            };
        });
    }
}
