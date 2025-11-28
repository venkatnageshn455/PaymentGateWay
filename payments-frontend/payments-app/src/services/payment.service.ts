import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Payment, PaymentRequestDto } from '../models/payment.model';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private baseUrl = `${environment.apiUrl}/api/Payments`;

  constructor(private http: HttpClient) {}

  getAll(userId?: string): Observable<Payment[]> {
    const url = userId ? `${this.baseUrl}/Get?userId=${encodeURIComponent(userId)}` : `${this.baseUrl}/Get`;
    return this.http.get<{ response: Payment[] }>(url).pipe(
      map(x => x.response),
      catchError(this.handleError)
    );
  }

  getById(userId: string, id: number): Observable<Payment> {
    return this.http.get<Payment>(`${this.baseUrl}/GetById?userId=${userId}&id=${id}`).pipe(
      catchError(this.handleError)
    );
  }

  create(payment: Payment): Observable<Payment> {
    return this.http.post<Payment>(`${this.baseUrl}/Create`, payment).pipe(catchError(this.handleError));
  }

  update(payment: PaymentRequestDto): Observable<Payment> {
    return this.http.put<Payment>(`${this.baseUrl}/Update`, payment).pipe(catchError(this.handleError));
  }

  delete(userId: string, id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Delete?userId=${userId}&id=${id}`).pipe(catchError(this.handleError));
  }

  getAllUsers(): Observable<string[]> {
    return this.http.get<{ response: string[] }>(`${this.baseUrl}/Users`).pipe(
      map(res => res.response ?? []),
      catchError(this.handleError)
    );
  }

  private handleError(err: any) {
    console.error('PaymentService error', err);
    return throwError(() => err);
  }
}
