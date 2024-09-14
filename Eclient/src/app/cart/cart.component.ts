import { Component, OnInit } from '@angular/core';
import { async, Observable } from 'rxjs';
import { ICart, ICartItem, ICartTotals } from '../shared/models/cart';
import { CartService } from './cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
cart$:Observable<ICart>;
cartTotals$: Observable<ICartTotals>;
//total:number;

  constructor(private cartService:CartService) { }

  ngOnInit(): void {
    this.cart$=this.cartService.basket$;
    this.cartTotals$ = this.cartService.cartTotal$;
  }

  removeCartItem(item:ICartItem){
  this.cartService.removeItemFromCart(item);
  }

  incrementQuantity(item:ICartItem){
    this.cartService.incrementQuantity(item);
  }

  decrementQuantity(item:ICartItem){
    this.cartService.decrementQuantity(item);
  }

}
