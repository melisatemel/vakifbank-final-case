import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlankComponent } from './layouts/blank/blank.component';
import { FullComponent } from './layouts/full/full.component';
import { ProductDetailComponent } from './pages/user/product-detail/product-detail.component';
import { ShoppingCartComponent } from './pages/user/shopping-cart/shopping-cart.component';
import { MyOrdersComponent } from './pages/user/my-orders/my-orders.component';
import { MyProfileComponent } from './pages/user/my-profile/my-profile.component';
import { AppAdminMessagesComponent } from './pages/admin/admin-messages/admin-messages.component';
import { AppAdminDashboardComponent } from './pages/admin/admin-dashboard/admin-dashboard.component';
import { AppAdminOrdersComponent } from './pages/admin/admin-orders/admin-orders.component';
import { AppAdminAddProductComponent } from './pages/admin/admin-add-product/admin-add-product.component';
import { AppAdminCustomizeProductsComponent } from './pages/admin/admin-customize-products/admin-customize-products.component';
import { AuthGuardService } from './services/auth-guard.service';
import { AdminGuardService } from './services/admin-guard.service';

const routes: Routes = [
  {
    path: '',
    component: FullComponent,
    children: [
      {
        path: '',
        redirectTo: '/authentication/login',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        canActivate: [AuthGuardService],
        loadChildren: () =>
          import('./pages/pages.module').then((m) => m.PagesModule),
      },
      {
        path: 'admin',
        canActivate: [AdminGuardService],
        children: [
          {
            path: 'dashboard',
            component: AppAdminDashboardComponent,
          },
          {
            path: 'messages',
            component: AppAdminMessagesComponent,
          },
          {
            path: 'orders',
            component: AppAdminOrdersComponent,
          },
          {
            path: 'add-product',
            component: AppAdminAddProductComponent,
          },
          {
            path: 'customize-products',
            component: AppAdminCustomizeProductsComponent,
          },
        ],
      },
      {
        path: 'product-detail/:id',
        canActivate: [AuthGuardService],
        component: ProductDetailComponent,
        data: {
          title: 'Product Detail Page',
        },
      },
      {
        path: 'my-profile',
        canActivate: [AuthGuardService],
        component: MyProfileComponent,
        data: {
          title: 'My Profile Page',
        },
      },
      {
        path: 'my-orders',
        canActivate: [AuthGuardService],
        component: MyOrdersComponent,
        data: {
          title: 'My Orders Page',
        },
      },
      {
        path: 'shopping-cart',
        canActivate: [AuthGuardService],
        component: ShoppingCartComponent,
        data: {
          title: 'Shopping Cart Page',
        },
      },
      {
        path: 'ui-components',
        canActivate: [AuthGuardService],
        loadChildren: () =>
          import('./pages/ui-components/ui-components.module').then(
            (m) => m.UicomponentsModule
          ),
      },
    ],
  },
  {
    path: '',
    component: BlankComponent,
    children: [
      {
        path: 'authentication',
        loadChildren: () =>
          import('./pages/authentication/authentication.module').then(
            (m) => m.AuthenticationModule
          ),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
