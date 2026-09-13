import { Service, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";

export interface CustomerResponse {
  id: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  createdAt: string;
}

export interface CustomerConsultation {
  id: number;
  customerId: number;
  notes: string | null;
  status: "InProgress" | "Completed";
  createdAt: string;
  completedAt: string | null;
}

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
    return this.http.get<CustomerResponse>(`${this.apiUrl}/${id}`);
  }

  searchCustomers(searchTerm: string) {
    return this.http.get<CustomerResponse[]>(this.apiUrl, {
      params: {
        search: searchTerm,
      },
    });
  }

  getCustomerConsultations(customerId: number) {
    return this.http.get<CustomerConsultation[]>(
      `${this.apiUrl}/${customerId}/consultations`,
    );
  }
}
