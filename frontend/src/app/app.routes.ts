import { Routes } from '@angular/router';
import { Home } from './features/home/home/home';
import { CustomerForm } from './features/customer/customer-form/customer-form';
import { ConsultationWorkspace } from './features/consultation/consultation-workspace/consultation-workspace';
import { Login } from './features/auth/login/login';
import { authGuard } from './core/auth/auth-guard';

export const routes: Routes = [
  {
    path: '',
    component: Home,
  },
  {
    path: 'login',
    component: Login,
  },
  {
    path: 'consultation/new',
    component: CustomerForm,
    canActivate: [authGuard],
  },
  {
    path: 'consultations/:id',
    component: ConsultationWorkspace,
    canActivate: [authGuard],
  },
];