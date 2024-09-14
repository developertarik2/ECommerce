import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { IProduct } from './shared/models/product';
import { CartService } from './cart/cart.service';
import { AccountService } from './account/account.service';
//import { IPagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'E-Commerce';
  products: IProduct[] ;
  //isCollapsed = true;
  constructor(private cartService:CartService,private accountService:AccountService){}
  ngOnInit(): void {

    this.loadCart();
    this.loadCurrentUser();
  /*  this.http.get('https://localhost:5001/api/products').subscribe(
      (response: any ) => {
   // console.log(response);
   this.products = response.data;
  }, error => {
    console.log(error);
  }); */
  }

  loadCurrentUser(){
  const token =localStorage.getItem('token');
  //if(token){
    this.accountService.loadCurrentUser(token).subscribe(()=>{
      console.log("loaded user");
    }, error => {
      console.log(error);
    })
  //}
  }
  loadCart(){
    const cartId = localStorage.getItem('basket_id');
    if(cartId){
      this.cartService.getBasket(cartId).subscribe(()=> {
       console.log('initialised basket');
      // console.log(cartId);
      // console.log(this.cartService.getBasket(cartId))
      }, error => {
       console.log(error);
      });
    }
  }
 
}
