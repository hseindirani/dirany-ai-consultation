import { Routes } from '@angular/router';
import { Home } from './features/home/home/home';
import { CustomerForm } from './features/customer/customer-form/customer-form';
import { ConsultationWorkspace } from './features/consultation/consultation-workspace/consultation-workspace';

export const routes: Routes = [
  {
    path: '',
    component: Home,
  },
  {
    path: 'consultation/new',
    component: CustomerForm,
  },
  {
    path: 'consultations/:id',
    component: ConsultationWorkspace,
  },
];