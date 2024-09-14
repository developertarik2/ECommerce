import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Cart, ICart, ICartItem, ICartTotals } from '../shared/models/cart';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {
baseUrl = environment.apiUrl;
private basketSource = new BehaviorSubject<ICart>(null);
basket$ = this.basketSource.asObservable();
private cartTotalSource=new BehaviorSubject<ICartTotals>(null);
cartTotal$=this.cartTotalSource.asObservable();
shipping=0;

  constructor(private http: HttpClient) { }


  createPaymentIntent(){
    return this.http.post<any>(this.baseUrl+'payments/'+this.getCurrentBasketValue().id,{})
    .pipe(
      map((cart:ICart)=>{
        this.basketSource.next(cart);
        console.log(this.getCurrentBasketValue());
      })
    )
  }
  getBasket(id:string){
  return this.http.get<any>(this.baseUrl+ 'basket?id=' + id)
  .pipe(
    map((basket:ICart) => {
      this.basketSource.next(basket);
      this.shipping=basket.shippingPrice;
      this.calculateTotals();
      //console.log(this.getCurrentBasketValue())
     // this.calculateTotals();
    })
  )
  }

  setBasket(basket: ICart) {
    return this.http.post<ICart>(this.baseUrl + 'basket', basket).subscribe((response: ICart) => {
      this.basketSource.next(response);
      console.log(response);
      this.calculateTotals();
    }, error => {
      console.log(error);
    });
    }
    getCurrentBasketValue() {
      return this.basketSource.value;
    }
    
    incrementQuantity(item:ICartItem){
    const cart = this.getCurrentBasketValue();
    const foundItemIndex=cart.items.findIndex(x=>x.id==item.id);
    cart.items[foundItemIndex].quantity++;
    this.setBasket(cart);
    }

    decrementQuantity(item:ICartItem){
      const cart = this.getCurrentBasketValue();
      const foundItemIndex=cart.items.findIndex(x=>x.id==item.id);
      if(cart.items[foundItemIndex].quantity>1){
        cart.items[foundItemIndex].quantity--;
      }
      else{
        this.removeItemFromCart(item); /// without redis
      }
      this.setBasket(cart);
      }
  removeItemFromCart(item: ICartItem) {
    const cart = this.getCurrentBasketValue();
    const foundItemIndex=cart.items.findIndex(x=>x.id==item.id);


  /*  if (cart.items.some(x => x.id === item.id)) {
      cart.items = cart.items.filter(i => i.id !== item.id);
      if (cart.items.length > 0) {*/

    if (foundItemIndex > -1) {
      cart.items.splice(foundItemIndex, 1);

      if(cart.items.length>0){
        this.setBasket(cart);
      } else {
        this.deleteCart(cart);
      }
   }
  }

  deleteLocalCart(id: string) {
    this.basketSource.next(null);
    this.cartTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }
  deleteCart(cart: ICart) {
    return this.http.delete(this.baseUrl+'basket?id='+cart.id).subscribe(()=>{
     this.basketSource.next(null);
     this.cartTotalSource.next(null);
     localStorage.removeItem('basket_id');
    },error => {
     console.log(error);
    });
  }
   
  setShippingPrice(method:IDeliveryMethod){
  this.shipping=method.price;
  const cart =this.getCurrentBasketValue();
  cart.deliveryMethodId=method.id;
  cart.shippingPrice=method.price;
  this.calculateTotals();
  this.setBasket(cart);
  }
    private calculateTotals(){
      const cart =this.getCurrentBasketValue();
      const shipping=this.shipping;
      const subtotal= cart.items.reduce((a,b)=> (b.price*b.quantity)+a,0);
      const total= shipping+subtotal;
      this.cartTotalSource.next({shipping,total,subtotal});
    }

    addItemToBasket(item:IProduct,quantity=1){
     const itemToAdd:ICartItem=this.mapProductItemItemToBasket(item,quantity);
     const basket=this.getCurrentBasketValue() ?? this.createBasket();
    // console.log(basket);
     basket.items =this.adOrUpdateItem(basket.items,itemToAdd,quantity);
     this.setBasket(basket);
   //  console.log(basket);
    }
  adOrUpdateItem(items: ICartItem[], itemToAdd: ICartItem, quantity: number): ICartItem[] {
   // console.log(itemToAdd);
    const index = items.findIndex(i=>i.id === itemToAdd.id);
    if(index === -1){
      itemToAdd.quantity=quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }
  createBasket(): ICart {
    const basket =new Cart();
    localStorage.setItem('basket_id',basket.id);
    return basket;
  }


    mapProductItemItemToBasket(item: IProduct, quantity: number): ICartItem {
    return {
      id:item.id,
      productName:item.name,
      price:item.price,
      pictureUrl:item.pictureUrl,
      quantity,
      brand:item.brand,
      category:item.category
    };
  }
}
