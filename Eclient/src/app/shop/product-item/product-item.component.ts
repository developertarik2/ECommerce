import { Component, Input, OnInit } from '@angular/core';
import { CartService } from 'src/app/cart/cart.service';
import { IProduct } from 'src/app/shared/models/product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent implements OnInit {
  @Input() product: IProduct;

  isBtn=false;
  constructor(private cartService:CartService) { }

  ngOnInit(): void {
    if (window.screen.width < 560){
      this.isBtn=true;
    }
  }

  addItemToCart(){
    this.cartService.addItemToBasket(this.product);
  }

}
