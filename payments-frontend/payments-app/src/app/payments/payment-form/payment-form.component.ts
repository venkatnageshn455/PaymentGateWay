import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaymentService } from '../../../services/payment.service';
import { Payment, PaymentRequestDto } from '../../../models/payment.model';

@Component({
  selector: 'app-payment-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './payment-form.component.html',
  styleUrls: ['./payment-form.component.scss']
})
export class PaymentFormComponent implements OnInit {

  @Input() paymentId?: number;
  @Input() userId?: string;
  @Output() close = new EventEmitter<boolean>();

  users: string[] = [];
  isEditMode = false;
  isSubmitting = false;

  // CREATE model → uses Payment
  createModel: Payment = {
    id: 0,
    userId: '',
    clientRequestId: crypto.randomUUID(),
    reference: '',
    amount: 0,
    currency: 'INR',
    createdAt: ''
  };

  // EDIT model → uses PaymentRequestDto
  editModel: PaymentRequestDto = {
    id: 0,
    userId: '',
    amount: 0,
    currency: 'INR',
    reference: ''
  };

  constructor(private svc: PaymentService) {}

  ngOnInit(): void {
    this.loadUsers();

    if (this.paymentId && this.userId) {
      this.isEditMode = true;
      this.loadForEdit(this.userId, this.paymentId);
    }
  }

  // Getter to unify model for template
  get model() {
    return this.isEditMode ? this.editModel : this.createModel;
  }

  loadUsers() {
    this.svc.getAllUsers().subscribe({
      next: (data) => this.users = data,
      error: (err) => console.error("Error loading users:", err)
    });
  }

  loadForEdit(userId: string, id: number) {
    this.svc.getById(userId, id).subscribe({
      next: (data) => {
        this.editModel = {
          id: data.id,
          userId: data.userId,
          amount: data.amount,
          currency: data.currency,
          reference: data.reference
        };
      },
      error: (err) => console.error("Error loading payment:", err)
    });
  }

  submitForm() {
    this.isSubmitting = true;

    if (this.isEditMode) {
      this.svc.update(this.editModel).subscribe({
        next: () => this.close.emit(true),
        error: (err) => { console.error(err); this.isSubmitting = false; }
      });
    } else {
      this.createModel.clientRequestId = crypto.randomUUID();
      this.createModel.userId = this.createModel.userId || this.users[0] || '';
      this.svc.create(this.createModel).subscribe({
        next: () => this.close.emit(true),
        error: (err) => { console.error(err); this.isSubmitting = false; }
      });
    }
  }

  cancel() {
    this.close.emit(false);
  }
}
