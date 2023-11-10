import { Routes } from '@angular/router';
import { AppDashboardComponent } from './user/dashboard/dashboard.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    component: AppDashboardComponent,
    data: {
      title: 'Starter Page',
    },
  }
];
