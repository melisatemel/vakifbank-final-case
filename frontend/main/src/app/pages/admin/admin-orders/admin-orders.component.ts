import { Component, OnInit, ViewEncapsulation} from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './admin-orders.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AppAdminOrdersComponent implements OnInit{
  showOnlyUncompletedOrders: boolean = false;
  orders : any = [];
  constructor(private router: Router, private shoppingCartService: ShoppingCartService, private toastr: ToastrService){}

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders(){
    this.shoppingCartService.getAllShoppingCartItems().subscribe((data: any)=>{
      this.orders = data.response;
    })
  }

  onCheckboxChange() {
    if (this.showOnlyUncompletedOrders) {
      this.orders = this.orders.filter((data: any)=> data.isActive === true)
    }else{
      this.getOrders();
    }
  }

  backToDashboard(){
    this.router.navigate(['admin/dashboard'])
  }

  calculateTotalPrice(order: any): number {
    let totalPrice = 0;

    order.productQuantities.forEach((product: any) => {
      totalPrice += product.price * product.quantity;
    });

    return totalPrice;
  }

  completeOrder(order: any){
    console.log(order)
    this.shoppingCartService.complateShoppingCart(order.id).subscribe((data: any)=>{
      this.getOrders();
   })
  }

  orderCompletedMessage(){
    this.toastr.info("You have already completed this order.");
  }
}
