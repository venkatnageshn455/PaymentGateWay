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

  @Input() row: Payment | null = null;
  @Output() close = new EventEmitter<boolean>();

  users: string[] = [];
  isEditMode = false;
  isSubmitting = false;

  selectedUser = '';
  showUserIdInput = true;

  createModel: Payment = {
    id: 0,
    userId: '',
    clientRequestId: crypto.randomUUID(),
    reference: '',
    amount: 0,
    currency: 'INR',
    createdAt: ''
  };

  editModel: PaymentRequestDto = {
    id: 0,
    userId: '',
    amount: 0,
    currency: 'INR',
    reference: '',
    updatedAt: ''
  };

  constructor(private svc: PaymentService) {}

  ngOnInit(): void {
    this.loadUsers();

    if (this.row) {
      this.isEditMode = true;
      this.setEditModel();
      this.selectedUser = this.row.userId;
      this.showUserIdInput = false; // user already selected
    }
  }

  get model() {
    return this.isEditMode ? this.editModel : this.createModel;
  }

  loadUsers() {
    this.svc.getAllUsers().subscribe({
      next: (data) => this.users = data,
      error: (err) => console.error("Error loading users:", err)
    });
  }

  setEditModel() {
    this.editModel = {
      id: this.row!.id,
      userId: this.row!.userId,
      amount: this.row!.amount,
      currency: this.row!.currency,
      reference: this.row!.reference,
      updatedAt: ''
    };
  }

  onUserSelect() {
    if (this.selectedUser) {
      this.model.userId = this.selectedUser;
      this.showUserIdInput = false;
    } else {
      this.model.userId = '';
      this.showUserIdInput = true;
    }
  }

  submitForm() {
    this.isSubmitting = true;

    if (this.isEditMode) {
      this.editModel.updatedAt = new Date().toISOString();

      this.svc.update(this.editModel).subscribe({
        next: () => this.close.emit(true),
        error: (err) => { console.error(err); this.isSubmitting = false; }
      });

    } else {
      this.createModel.clientRequestId = crypto.randomUUID();

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
