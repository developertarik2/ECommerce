import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICart } from '../shared/models/cart';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IOrder, IOrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
baseUrl=environment.apiUrl;

  constructor(private http:HttpClient) { }

  getDeliveryMethods(){
    return this.http.get<any>(this.baseUrl+'orders/deliveryMethods').pipe(
      map((dm: IDeliveryMethod[])=> {
        return dm.sort((a,b)=>b.price-a.price);
      })
    );
  }

  createOrder(order: IOrderToCreate) {
    return this.http.post<IOrder>(this.baseUrl + 'orders', order);
   }
}
