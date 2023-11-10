import { Component, ViewEncapsulation} from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AppAdminDashboardComponent {

  constructor(private router: Router){

  }

  navigateToMessages() {
    this.router.navigate(['admin/messages'])
  }

  navigateToOrders() {
    this.router.navigate(['admin/orders'])
  }

  navigateToCustomizeProducts() {
    this.router.navigate(['admin/customize-products'])
  }

  navigateToAddProduct() {
    this.router.navigate(['admin/add-product'])
  }
}
