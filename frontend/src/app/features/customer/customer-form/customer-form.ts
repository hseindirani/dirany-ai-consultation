import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Customer } from '../../../core/services/customer';

@Component({
  imports: [RouterLink, ReactiveFormsModule],
  selector: 'app-customer-form',
  styleUrl: './customer-form.css',
  templateUrl: './customer-form.html',
})
export class CustomerForm {
  isSubmitting = false;

  customerForm = new FormGroup({
    firstName: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    lastName: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
    phoneNumber: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),
  });

  constructor(private customerService: Customer) {}

  onSubmit() {
    if (this.customerForm.invalid || this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;

    const customer = this.customerForm.getRawValue();

    this.customerService.createCustomer(customer).subscribe({
      next: (response) => {
        console.log('Customer created:', response);
        this.isSubmitting = false;
      },
      error: (error) => {
        console.error('Customer creation failed:', error);
        this.isSubmitting = false;
      },
    });
  }
}