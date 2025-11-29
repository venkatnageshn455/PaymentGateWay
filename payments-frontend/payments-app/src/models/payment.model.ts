export interface Payment {
  id: number;
  userId: string;
  clientRequestId: string | null;
  amount: number;
  currency: string;
  reference: string;
  createdAt?: string;
  updatedAt?: string | null;
}

export interface PaymentRequestDto {
  id: number;
  userId: string;
  amount: number;
  currency: string;
  reference?: string;
  updatedAt?: string | null;

}

export interface UserDto {
  name: string;
}
