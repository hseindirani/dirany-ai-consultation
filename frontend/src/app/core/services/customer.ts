import { Service, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Service()
export class Customer {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/customers`;

  createCustomer(customer: {
    firstName: string;
    lastName: string;
    phoneNumber: string;
  }) {
    return this.http.post(this.apiUrl, customer);
  }

  getCustomer(id: number) {
    return this.http.get(`${this.apiUrl}/${id}`);
  }
}