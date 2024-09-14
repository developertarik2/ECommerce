import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CartService } from 'src/app/cart/cart.service';
import { ICartTotals } from '../../models/cart';

@Component({
  selector: 'app-order-totals',
  templateUrl: './order-totals.component.html',
  styleUrls: ['./order-totals.component.scss']
})
export class OrderTotalsComponent implements OnInit {
  @Input() shippingPrice: number;
  @Input() subtotal: number;
  @Input() total: number;
 
cartTotal$:Observable<ICartTotals>;

  constructor(private cartService:CartService) { }

  ngOnInit(): void {
    this.cartTotal$=this.cartService.cartTotal$;
  }

}
