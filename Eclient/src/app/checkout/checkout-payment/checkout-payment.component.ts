import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CartService } from 'src/app/cart/cart.service';
import { ICart } from 'src/app/shared/models/cart';
import { IOrder } from 'src/app/shared/models/order';
import { CheckoutService } from '../checkout.service';

declare var Stripe:any;

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements AfterViewInit,OnDestroy {
@Input() checkoutForm:FormGroup;
@ViewChild('cardNumber',{static:true})cardNumberElemnet:ElementRef;
@ViewChild('cardExpiry',{static:true})cardExpiryElemnet:ElementRef;
@ViewChild('cardCvc',{static:true})cardCvcElemnet:ElementRef;

stripe:any;
cardNumber:any;
cardExpiry:any;
cardCvc:any;
cardErrors:any;
cardHandler=this.onChange.bind(this);
loading=false;
cardNumberValid=false;
cardExpiryValid=false;
cardCvcValid=false;

  constructor(private cartService:CartService,private checkoutServie:CheckoutService,
              private toastr:ToastrService,private router:Router) { }
  
  
  ngOnDestroy(): void {
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
  }

  //onChange({error}:any){
    onChange( event:any){
    //  console.log(event);
   if(event.error){
     this.cardErrors=event.error.message;
   }
   else{
     this.cardErrors=null;
   }
   switch(event.elementType){
     case 'cardNumber':
       this.cardNumberValid=event.complete;
       break;
       case 'cardExpiry':
        this.cardExpiryValid=event.complete;
        break;
        case 'cardCvc':
          this.cardCvcValid=event.complete;
          break;
   }
  }

  ngAfterViewInit() {
    this.stripe= Stripe('pk_test_51Iy8z3Gzwp1dz9Fy3pKialdlkzRQUQkh2Owo1ntY8U3ReXcW7VVmeeXl0fvju2twOekEYMTlsUslanFg0CPKM3Dp00lcjyXJPl');
    const elements= this.stripe.elements();

    this.cardNumber=elements.create('cardNumber');
    this.cardNumber.mount(this.cardNumberElemnet.nativeElement);
    this.cardNumber.addEventListener('change',this.cardHandler);

    this.cardExpiry=elements.create('cardExpiry');
    this.cardExpiry.mount(this.cardExpiryElemnet.nativeElement);
    this.cardExpiry.addEventListener('change',this.cardHandler);

    this.cardCvc=elements.create('cardCvc');
    this.cardCvc.mount(this.cardCvcElemnet.nativeElement);
    this.cardCvc.addEventListener('change',this.cardHandler);
  }

  async submitOrder(){
    this.loading=true;
    const cart =this.cartService.getCurrentBasketValue();

    try {
      const createdOrder=await this.createOrder(cart);
      //console.log(this.checkoutForm.get('deliveryForm').get('deliveryMethod').value);
      const paymentResult= await this.confirmPaymentWithStripe(cart);
  
      if(paymentResult.paymentIntent){
       // this.cartService.deleteLocalCart(cart.id);
       this.cartService.deleteCart(cart);
        const navigationExtras:NavigationExtras={state:createdOrder};
        this.router.navigate(['checkout/success'],navigationExtras);
      } else {
       // this.toastr.error('Payment error');
       this.toastr.error(paymentResult.error.message);
      }
      this.loading=false;
    }
    catch(error) {
     console.log(error);
     this.loading=false;
    }
    


   /* this.checkoutServie.createOrder(orderToCreate).subscribe((order:IOrder) => {
    this.toastr.success('Order created');
    
    this.stripe.confirmCardPayment(cart.clientSecret,{
      payment_method:{
        card:this.cardNumber,
        billing_details:{
          name:this.checkoutForm.get('paymentForm').get('nameOnCard').value
        }
      }
    }).then((result: any)=>{
      console.log(result);
    /*  if(result.paymentIntent){
        this.cartService.deleteLocalCart(cart.id);
        const navigationExtras:NavigationExtras={state:order};
        this.router.navigate(['checkout/success'],navigationExtras);
      } else {
       // this.toastr.error('Payment error');
       this.toastr.error(result.error.message);
      }
    })
    
      //const navigationExtras:NavigationExtras={state:order};
      //this.router.navigate(['checkout/success'],navigationExtras);
     // this.cartService.deleteLocalCart(cart.id);
      console.log(order);
    }, error=> {
      this.toastr.error(error.message);
      console.log(error);
    })*/
  }
  
 private async confirmPaymentWithStripe(cart:any) {
 return this.stripe.confirmCardPayment(cart.clientSecret,{
    payment_method:{
      card:this.cardNumber,
      billing_details:{
        name:this.checkoutForm.get('paymentForm').get('nameOnCard').value
      }
    }
  });
  }
  
  private async createOrder(cart: ICart) {
    const orderToCreate=this.getOrderToCreate(cart);
   return this.checkoutServie.createOrder(orderToCreate).toPromise();
  }

  getOrderToCreate(cart: ICart) {
    return {
      cartId: cart.id,
    deliveryMethodId: this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
    shipToAddress: this.checkoutForm.get('addressForm').value
    }
  }

}
