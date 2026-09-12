import { Service, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";

export interface HairCandidate {
  id: number;
  consultationId: number;
  hairStyleId: number;
  isSelected: boolean;
  createdAt: string;
}

export interface BeardCandidate {
  id: number;
  consultationId: number;
  beardStyleId: number;
  isSelected: boolean;
  createdAt: string;
}
export interface ConsultationImage {
  id: number;
  consultationId: number;
  hairCandidateId: number | null;
  beardCandidateId: number | null;
  imageType: string;
  createdAt: string;
}

@Service()
export class Consultation {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  createConsultation(customerId: number) {
    return this.http.post(
      `${this.apiUrl}/customers/${customerId}/consultations`,
      {},
    );
  }

  getConsultation(id: number) {
    return this.http.get(`${this.apiUrl}/consultations/${id}`);
  }

  uploadOriginalImage(consultationId: number, file: File) {
    const formData = new FormData();
    formData.append("file", file);

    return this.http.post(
      `${this.apiUrl}/consultations/${consultationId}/images/original`,
      formData,
    );
  }

  addHairCandidate(consultationId: number, hairStyleId: number) {
    return this.http.post<HairCandidate>(
      `${this.apiUrl}/consultations/${consultationId}/hair-candidates`,
      {
        hairStyleId,
      },
    );
  }

  getHairCandidates(consultationId: number) {
    return this.http.get<HairCandidate[]>(
      `${this.apiUrl}/consultations/${consultationId}/hair-candidates`,
    );
  }

  selectHairCandidate(consultationId: number, candidateId: number) {
    return this.http.put<HairCandidate>(
      `${this.apiUrl}/consultations/${consultationId}/hair-candidates/${candidateId}/selection`,
      {},
    );
  }

  removeHairCandidate(consultationId: number, candidateId: number) {
    return this.http.delete<void>(
      `${this.apiUrl}/consultations/${consultationId}/hair-candidates/${candidateId}`,
    );
  }

  addBeardCandidate(consultationId: number, beardStyleId: number) {
    return this.http.post<BeardCandidate>(
      `${this.apiUrl}/consultations/${consultationId}/beard-candidates`,
      {
        beardStyleId,
      },
    );
  }

  getBeardCandidates(consultationId: number) {
    return this.http.get<BeardCandidate[]>(
      `${this.apiUrl}/consultations/${consultationId}/beard-candidates`,
    );
  }

  selectBeardCandidate(consultationId: number, candidateId: number) {
    return this.http.put<BeardCandidate>(
      `${this.apiUrl}/consultations/${consultationId}/beard-candidates/${candidateId}/selection`,
      {},
    );
  }

  removeBeardCandidate(consultationId: number, candidateId: number) {
    return this.http.delete<void>(
      `${this.apiUrl}/consultations/${consultationId}/beard-candidates/${candidateId}`,
    );
  }
  getImages(consultationId: number) {
    return this.http.get<ConsultationImage[]>(
      `${this.apiUrl}/consultations/${consultationId}/images`,
    );
  }

  getImageUrl(consultationId: number, imageId: number) {
    return `${this.apiUrl}/consultations/${consultationId}/images/${imageId}`;
  }

  generateHairPreview(consultationId: number, candidateId: number) {
    return this.http.post<{ imageId: number; hairCandidateId: number }>(
      `${this.apiUrl}/consultations/${consultationId}/hair-candidates/${candidateId}/preview`,
      {},
    );
  }
  generateBeardPreview(consultationId: number, candidateId: number) {
  return this.http.post<{ imageId: number; beardCandidateId: number }>(
    `${this.apiUrl}/consultations/${consultationId}/beard-candidates/${candidateId}/preview`,
    {},
  );
}
}
