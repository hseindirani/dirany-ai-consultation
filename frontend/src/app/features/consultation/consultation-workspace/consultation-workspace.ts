import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Consultation } from '../../../core/services/consultation';
import { Customer } from '../../../core/services/customer';

@Component({
  selector: 'app-consultation-workspace',
  imports: [],
  templateUrl: './consultation-workspace.html',
  styleUrl: './consultation-workspace.css',
})
export class ConsultationWorkspace implements OnInit {
  consultation = signal<any>(null);
  customer = signal<any>(null);

  selectedFile = signal<File | null>(null);
  selectedFileName = signal('');
  isUploading = signal(false);
  uploadSuccess = signal(false);

  previewUrl: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private consultationService: Consultation,
    private customerService: Customer
  ) {}

  ngOnInit() {
    const consultationId = Number(this.route.snapshot.paramMap.get('id'));

    this.consultationService.getConsultation(consultationId).subscribe({
      next: (consultation: any) => {
        this.consultation.set(consultation);

        this.customerService.getCustomer(consultation.customerId).subscribe({
          next: (customer) => {
            this.customer.set(customer);
          },
          error: (error) => {
            console.error('Failed to load customer:', error);
          },
        });
      },
      error: (error) => {
        console.error('Failed to load consultation:', error);
      },
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    this.selectedFile.set(file);
    this.selectedFileName.set(file.name);
    this.uploadSuccess.set(false);

    if (this.previewUrl) {
      URL.revokeObjectURL(this.previewUrl);
    }

    this.previewUrl = URL.createObjectURL(file);
  }

  uploadPhoto() {
    const consultation = this.consultation();
    const selectedFile = this.selectedFile();

    if (!consultation || !selectedFile || this.isUploading()) {
      return;
    }

    this.isUploading.set(true);
    this.uploadSuccess.set(false);

    this.consultationService
      .uploadOriginalImage(consultation.id, selectedFile)
      .subscribe({
        next: (image) => {
          console.log('Original image uploaded:', image);

          this.isUploading.set(false);
          this.uploadSuccess.set(true);
        },
        error: (error) => {
          console.error('Image upload failed:', error);
          this.isUploading.set(false);
        },
      });
  }
}