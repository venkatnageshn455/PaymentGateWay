import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Payment } from '../../../models/payment.model';
import { PaymentService } from '../../../services/payment.service';
import { PaymentFormComponent } from '../payment-form/payment-form.component';

@Component({
  selector: 'app-payment-list',
  standalone: true,
  imports: [CommonModule, FormsModule, PaymentFormComponent],
  templateUrl: './payment-list.component.html',
  styleUrls: ['./payment-list.component.scss']
})
export class PaymentListComponent implements OnInit {

  users: string[] = [];
  selectedUser: string = '';
  payments: Payment[] = [];
  loading: boolean = false;

  // Form state
  isFormVisible = false;
  selectedRow: Payment | null = null;   // NEW → passes whole row

  constructor(private paymentService: PaymentService) {}

  ngOnInit(): void {
    this.loadUsers();
    this.loadPayments();
  }

  loadUsers() {
    this.paymentService.getAllUsers().subscribe({
      next: res => this.users = res,
      error: err => console.error(err)
    });
  }

  loadPayments() {
    this.loading = true;
    this.paymentService.getAll(this.selectedUser).subscribe({
      next: res => {
        this.payments = res;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  onUserChange() {
    this.loadPayments();
  }

  openAddPayment() {
    this.selectedRow = null;        // no row → Add mode
    this.isFormVisible = true;
  }

  openEditPayment(payment: Payment) {
    this.selectedRow = payment;     // pass full row for Edit mode
    this.isFormVisible = true;
  }

  onFormClose(refresh: boolean) {
    this.isFormVisible = false;
    this.selectedRow = null;

    if (refresh) {
      this.loadPayments();
    }
  }

  deletePayment(userId: string, id: number) {
    if (!confirm("Delete this payment?")) return;

    this.paymentService.delete(userId, id).subscribe({
      next: () => this.loadPayments(),
      error: err => console.error(err)
    });
  }
}
