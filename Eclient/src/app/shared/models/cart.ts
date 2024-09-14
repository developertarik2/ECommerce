
import * as uuid from 'uuid';
export interface ICart {
    id: string;
    items: ICartItem[];
    clientSecret?: string;
    paymentIntentId?: string;
    deliveryMethodId?: number;
    shippingPrice?: number;
}
export interface ICartItem {
        id: number;
        productName: string;
        price: number;
        quantity: number;
        pictureUrl: string;
        brand: string;
        category: string;
    }
export class Cart implements ICart {
        id = uuid.v4();
        items: ICartItem[]=[];
    }
export interface ICartTotals {
        shipping: number;
        subtotal: number;
        total: number;
    }