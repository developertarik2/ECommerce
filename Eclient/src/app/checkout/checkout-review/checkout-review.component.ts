import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { CartService } from 'src/app/cart/cart.service';
import { ICart } from 'src/app/shared/models/cart';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {
@Input() appStepper:CdkStepper;

  cart$:Observable<ICart>;

  constructor(private cartService:CartService,private toastr:ToastrService) { }

  ngOnInit(): void {
   this.cart$=this.cartService.basket$;
  }

  createPaymentIntent(){
    return this.cartService.createPaymentIntent().subscribe((response:any)=> {
    //  this.toastr.success('Payment intent created');
      this.appStepper.next();
    }, error => {
      console.log(error);
    //  this.toastr.error(error.message);
    })
  }

}
