import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { CartService } from 'src/app/cart/cart.service';
//import { EventEmitter } from 'stream';
import { ICart, ICartItem } from '../../models/cart';
import { IOrderItem } from '../../models/order';

@Component({
  selector: 'app-cart-summary',
  templateUrl: './cart-summary.component.html',
  styleUrls: ['./cart-summary.component.scss']
})
export class CartSummaryComponent implements OnInit {
cart$:Observable<ICart>;
@Output() decrement:EventEmitter<ICartItem> =new EventEmitter<ICartItem>();
@Output() increment:EventEmitter<ICartItem> =new EventEmitter<ICartItem>();
@Output() remove:EventEmitter<ICartItem> =new EventEmitter<ICartItem>();



@Input() isCart:boolean= true;
//@Input() items: ICartItem[] | IOrderItem[] = [];
@Input() items: any;
@Input() isOrder = false;
  constructor(private cartService:CartService) { }

  ngOnInit(): void {
    this.cart$=this.cartService.basket$;
  }
  
  decrementQuantity(item:ICartItem){
   this.decrement.emit(item);
  }
  incrementQuantity(item:ICartItem){
    this.increment.emit(item);
   }
   removeCartItem(item:ICartItem){
    this.remove.emit(item);
   }

}
