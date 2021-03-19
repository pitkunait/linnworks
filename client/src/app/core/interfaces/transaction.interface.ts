export interface Transaction {
    id: number;
    asset: string;
    amountCrypto: number;
    amountCash: number;
    dateOfTransfer: string;
    currentValue: number;
    profitLoss: number;
    timeSeries: any[];
}
