export interface SalesRecord {
    id: number;
    region: string;
    country: string;
    itemType: string;
    salesChannel: string;
    orderPriority: string;
    orderDate: string;
    orderId: number;
    shipDate: string;
    unitsSold: string;
    unitPrice: number;
    unitCost: number;
    totalRevenue: number;
    totalCost: number;
    totalProfit: number;
}
