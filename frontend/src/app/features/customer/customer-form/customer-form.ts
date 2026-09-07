import { Component } from "@angular/core";
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { Router, RouterLink } from "@angular/router";

import { Customer } from "../../../core/services/customer";
import { Consultation } from "../../../core/services/consultation";

@Component({
  imports: [RouterLink, ReactiveFormsModule],
  selector: "app-customer-form",
  styleUrl: "./customer-form.css",
  templateUrl: "./customer-form.html",
})
export class CustomerForm {
  isSubmitting = false;

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
  ) {}

  onSubmit() {
    if (this.customerForm.invalid || this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;

    const customer = this.customerForm.getRawValue();

    this.customerService.createCustomer(customer).subscribe({
      next: (response: any) => {
        console.log("Customer created:", response);

        this.consultationService.createConsultation(response.id).subscribe({
          next: (consultation: any) => {
            console.log("Consultation created:", consultation);
            this.router.navigate(["/consultations", consultation.id]);
          },
          error: (error) => {
            console.error("Consultation creation failed:", error);
            this.isSubmitting = false;
          },
        });
      },
      error: (error) => {
        console.error("Customer creation failed:", error);
        this.isSubmitting = false;
      },
    });
  }
}
