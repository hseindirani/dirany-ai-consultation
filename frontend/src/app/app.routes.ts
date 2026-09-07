import { Routes } from '@angular/router';
import { Home } from './features/home/home/home';
import { CustomerForm } from './features/customer/customer-form/customer-form';

export const routes: Routes = [
  {
    path: '',
    component: Home,
  },
  {
    path: 'consultation/new',
    component: CustomerForm,
  },
];