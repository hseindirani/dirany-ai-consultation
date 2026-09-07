import { Service, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

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

@Service()
export class Consultation {
  private readonly http = inject(HttpClient);

  createConsultation(customerId: number) {
    return this.http.post(
      `http://localhost:5091/api/customers/${customerId}/consultations`,
      {},
    );
  }

  getConsultation(id: number) {
    return this.http.get(`http://localhost:5091/api/consultations/${id}`);
  }

  uploadOriginalImage(consultationId: number, file: File) {
    const formData = new FormData();
    formData.append("file", file);

    return this.http.post(
      `http://localhost:5091/api/consultations/${consultationId}/images/original`,
      formData,
    );
  }

  addHairCandidate(consultationId: number, hairStyleId: number) {
    return this.http.post<HairCandidate>(
      `http://localhost:5091/api/consultations/${consultationId}/hair-candidates`,
      {
        hairStyleId,
      },
    );
  }

  getHairCandidates(consultationId: number) {
    return this.http.get<HairCandidate[]>(
      `http://localhost:5091/api/consultations/${consultationId}/hair-candidates`,
    );
  }
  selectHairCandidate(consultationId: number, candidateId: number) {
    return this.http.put<HairCandidate>(
      `http://localhost:5091/api/consultations/${consultationId}/hair-candidates/${candidateId}/selection`,
      {},
    );
  }
  removeHairCandidate(consultationId: number, candidateId: number) {
    return this.http.delete<void>(
      `http://localhost:5091/api/consultations/${consultationId}/hair-candidates/${candidateId}`,
    );
  }
  addBeardCandidate(consultationId: number, beardStyleId: number) {
    return this.http.post<BeardCandidate>(
      `http://localhost:5091/api/consultations/${consultationId}/beard-candidates`,
      { beardStyleId },
    );
  }

  getBeardCandidates(consultationId: number) {
    return this.http.get<BeardCandidate[]>(
      `http://localhost:5091/api/consultations/${consultationId}/beard-candidates`,
    );
  }

  selectBeardCandidate(consultationId: number, candidateId: number) {
    return this.http.put<BeardCandidate>(
      `http://localhost:5091/api/consultations/${consultationId}/beard-candidates/${candidateId}/selection`,
      {},
    );
  }

  removeBeardCandidate(consultationId: number, candidateId: number) {
    return this.http.delete<void>(
      `http://localhost:5091/api/consultations/${consultationId}/beard-candidates/${candidateId}`,
    );
  }
}
