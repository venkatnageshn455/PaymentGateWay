export interface Payment {
  id: number;
  userId: string;
  clientRequestId: string | null;
  amount: number;
  currency: string;
  reference: string;
  createdAt?: string;
}

export interface PaymentRequestDto {
  id: number;
  userId: string;
  amount: number;
  currency: string;
  reference?: string;
}

export interface UserDto {
  name: string;
}
