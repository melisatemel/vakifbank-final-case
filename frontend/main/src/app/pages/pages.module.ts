import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PagesRoutes } from './pages.routing.module';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';
import { TablerIconsModule } from 'angular-tabler-icons';
import * as TablerIcons from 'angular-tabler-icons/icons';
import { AppDashboardComponent } from './user/dashboard/dashboard.component';
import { AuthService } from '../services/auth.service';
import { ProductDetailComponent } from './user/product-detail/product-detail.component';
import { ShoppingCartComponent } from './user/shopping-cart/shopping-cart.component';
import { MyOrdersComponent } from './user/my-orders/my-orders.component';
import { MyProfileComponent } from './user/my-profile/my-profile.component';
import { AppAdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';
import { AppAdminMessagesComponent } from './admin/admin-messages/admin-messages.component';
import { AppAdminOrdersComponent } from './admin/admin-orders/admin-orders.component';
import { AppAdminAddProductComponent } from './admin/admin-add-product/admin-add-product.component';
import { AppAdminCustomizeProductsComponent } from './admin/admin-customize-products/admin-customize-products.component';
@NgModule({
  declarations: [
    AppDashboardComponent,
    AppAdminMessagesComponent,
    AppAdminDashboardComponent,
    ProductDetailComponent,
    ShoppingCartComponent,
    MyOrdersComponent,
    MyProfileComponent,
    AppAdminOrdersComponent,
    AppAdminAddProductComponent,
    AppAdminCustomizeProductsComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    NgApexchartsModule,
    RouterModule.forChild(PagesRoutes),
    TablerIconsModule.pick(TablerIcons),
    ReactiveFormsModule,
  ],
  exports: [TablerIconsModule],
  providers: [AuthService],
})
export class PagesModule {}
