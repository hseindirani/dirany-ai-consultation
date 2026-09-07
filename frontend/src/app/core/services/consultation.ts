import { Service, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Service()
export class Consultation {
  private readonly http = inject(HttpClient);

  createConsultation(customerId: number) {
    return this.http.post(
      `http://localhost:5091/api/customers/${customerId}/consultations`,
      {}
    );
  }

  getConsultation(id: number) {
    return this.http.get(
      `http://localhost:5091/api/consultations/${id}`
    );
  }
  uploadOriginalImage(consultationId: number, file: File) {
  const formData = new FormData();
  formData.append('file', file);

  return this.http.post(
    `http://localhost:5091/api/consultations/${consultationId}/images/original`,
    formData
  );
}
}