import {
  Component,
  Output,
  EventEmitter,
  Input,
  ViewEncapsulation,
  OnInit,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';
import { StorageService } from 'src/app/services/storage.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class HeaderComponent implements OnInit {
  @Input() showToggle = true;
  @Input() toggleChecked = false;
  @Output() toggleMobileNav = new EventEmitter<void>();
  @Output() toggleMobileFilterNav = new EventEmitter<void>();
  @Output() toggleCollapsed = new EventEmitter<void>();
  userRole: any;
  numberOfItems: number = 0;
  showFiller = false;
  quantity: number;
  constructor(public dialog: MatDialog, private storage: StorageService, private router: Router, private shoppingCartService: ShoppingCartService, private authService: AuthService) {}

  logOut(){
    this.storage.clean();
    this.router.navigate(['/authentication/login']);
  }

  ngOnInit(): void {
    this.userRole = this.authService.getRole();
    this.shoppingCartService.updateNumberOfItems();
    this.shoppingCartService.numberOfItems$.subscribe((value: number) => {
      this.quantity = value;
    });

  }

  goToShoppingCart(){
    this.router.navigate(['/shopping-cart']);
  }

  navigateToMyOrders(){
    this.router.navigate(['/my-orders']);
  }

  navigateToMyProfile(){
    this.router.navigate(['/my-profile']);
  }
}
