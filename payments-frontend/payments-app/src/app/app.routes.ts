import { Routes } from '@angular/router';
import { PaymentListComponent } from './payments/payment-list/payment-list.component';
import { PaymentFormComponent } from './payments/payment-form/payment-form.component';


export const routes: Routes = [
{ path: '', redirectTo: 'payments', pathMatch: 'full' },
{ path: 'payments', component: PaymentListComponent },
{ path: 'payments/add', component: PaymentFormComponent },
{ path: 'payments/edit/:id', component: PaymentFormComponent }
];