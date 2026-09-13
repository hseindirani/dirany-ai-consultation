import { Component, OnInit, signal } from "@angular/core";
import { DatePipe } from "@angular/common";
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";

import {
  Customer,
  CustomerConsultation,
  CustomerResponse,
} from "../../../core/services/customer";
import { Consultation } from "../../../core/services/consultation";

@Component({
  imports: [RouterLink, ReactiveFormsModule, DatePipe],
  selector: "app-customer-form",
  styleUrl: "./customer-form.css",
  templateUrl: "./customer-form.html",
})
export class CustomerForm implements OnInit {
  isSubmitting = false;

  searchTerm = signal("");
  searchResults = signal<CustomerResponse[]>([]);
  isSearching = signal(false);
  searchError = signal<string | null>(null);

  private searchTimeout?: ReturnType<typeof setTimeout>;

  selectedCustomer = signal<CustomerResponse | null>(null);
  customerHistory = signal<CustomerConsultation[]>([]);
  isLoadingHistory = signal(false);
  historyError = signal<string | null>(null);

  customerForm = new FormGroup({
    firstName: new FormControl("", {
      nonNullable: true,
      validators: [Validators.required],
    }),
    lastName: new FormControl("", {
      nonNullable: true,
      validators: [Validators.required],
    }),
    phoneNumber: new FormControl("", {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  constructor(
    private customerService: Customer,
    private consultationService: Consultation,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit() {
    const customerId = Number(
      this.route.snapshot.queryParamMap.get("customerId"),
    );

    if (!customerId) return;

    this.customerService.getCustomer(customerId).subscribe({
      next: (customer) => {
        this.searchTerm.set(`${customer.firstName} ${customer.lastName}`);

        this.selectCustomer(customer);
      },
      error: (error) => {
        console.error("Failed to restore customer:", error);
      },
    });
  }

  onSearchInput(event: Event) {
    const input = event.target as HTMLInputElement;
    const searchTerm = input.value.trim();

    this.searchTerm.set(searchTerm);
    this.searchError.set(null);

    this.selectedCustomer.set(null);
    this.customerHistory.set([]);
    this.historyError.set(null);

    clearTimeout(this.searchTimeout);

    if (!searchTerm) {
      this.searchResults.set([]);
      this.isSearching.set(false);
      return;
    }

    this.isSearching.set(true);

    this.searchTimeout = setTimeout(() => {
      this.searchCustomers(searchTerm);
    }, 300);
  }

  searchCustomers(searchTerm: string) {
    this.isSearching.set(true);
    this.searchError.set(null);

    this.customerService.searchCustomers(searchTerm).subscribe({
      next: (customers) => {
        this.searchResults.set(customers);
        this.isSearching.set(false);
      },
      error: (error) => {
        console.error("Customer search failed:", error);

        this.searchResults.set([]);
        this.searchError.set("Could not search customers. Please try again.");

        this.isSearching.set(false);
      },
    });
  }

  selectCustomer(customer: CustomerResponse) {
    this.selectedCustomer.set(customer);
    this.searchResults.set([]);
    this.customerHistory.set([]);
    this.historyError.set(null);
    this.isLoadingHistory.set(true);

    this.customerService.getCustomerConsultations(customer.id).subscribe({
      next: (consultations) => {
        this.customerHistory.set(consultations);
        this.isLoadingHistory.set(false);
      },
      error: (error) => {
        console.error("Customer history failed:", error);

        this.historyError.set("Could not load consultation history.");

        this.isLoadingHistory.set(false);
      },
    });
  }

  onSubmit() {
    if (this.customerForm.invalid || this.isSubmitting) return;

    this.isSubmitting = true;

    const customer = this.customerForm.getRawValue();

    this.customerService.createCustomer(customer).subscribe({
      next: (response: any) => {
        console.log("Customer created:", response);

        this.isSubmitting = false;
        this.startConsultation(response.id);
      },
      error: (error) => {
        console.error("Customer creation failed:", error);
        this.isSubmitting = false;
      },
    });
  }

  startConsultation(customerId: number) {
    if (this.isSubmitting) return;

    this.isSubmitting = true;

    this.consultationService.createConsultation(customerId).subscribe({
      next: (consultation: any) => {
        console.log("Consultation created:", consultation);

        this.router.navigate(["/consultations", consultation.id]);
      },
      error: (error) => {
        console.error("Consultation creation failed:", error);

        this.isSubmitting = false;
      },
    });
  }

  openConsultation(consultationId: number) {
    this.router.navigate(["/consultations", consultationId]);
  }
}
