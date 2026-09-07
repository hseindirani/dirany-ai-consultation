import { Component, OnInit, computed, signal } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import {
  BeardCandidate,
  Consultation,
  HairCandidate,
} from "../../../core/services/consultation";
import { Customer } from "../../../core/services/customer";
import {
  StyleCatalog,
  StyleOption,
} from "../../../core/services/style-catalog";

@Component({
  selector: "app-consultation-workspace",
  imports: [],
  templateUrl: "./consultation-workspace.html",
  styleUrl: "./consultation-workspace.css",
})
export class ConsultationWorkspace implements OnInit {
  consultation = signal<any>(null);
  customer = signal<any>(null);

  hairStyles = signal<StyleOption[]>([]);
  hairCandidates = signal<HairCandidate[]>([]);
  beardStyles = signal<StyleOption[]>([]);
  beardCandidates = signal<BeardCandidate[]>([]);

  readonly maxHairCandidates = 3;
  readonly maxBeardCandidates = 3;

  hairStyleIndex = signal(0);
  beardStyleIndex = signal(0);

  currentHairStyle = computed(() => {
    return this.hairStyles()[this.hairStyleIndex()] ?? null;
  });
  currentBeardStyle = computed(() => {
    return this.beardStyles()[this.beardStyleIndex()] ?? null;
  });

  selectedFile = signal<File | null>(null);
  selectedFileName = signal("");
  isUploading = signal(false);
  uploadSuccess = signal(false);

  previewUrl: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private consultationService: Consultation,
    private customerService: Customer,
    private styleCatalogService: StyleCatalog,
  ) {}

  ngOnInit() {
    const consultationId = Number(this.route.snapshot.paramMap.get("id"));

    this.consultationService.getConsultation(consultationId).subscribe({
      next: (consultation: any) => {
        this.consultation.set(consultation);

        this.customerService.getCustomer(consultation.customerId).subscribe({
          next: (customer) => {
            this.customer.set(customer);
          },
          error: (error) => {
            console.error("Failed to load customer:", error);
          },
        });
      },
      error: (error) => {
        console.error("Failed to load consultation:", error);
      },
    });

    this.styleCatalogService.getHairStyles().subscribe({
      next: (styles) => {
        this.hairStyles.set(styles);
      },
      error: (error) => {
        console.error("Failed to load hair styles:", error);
      },
    });

    this.consultationService.getHairCandidates(consultationId).subscribe({
      next: (candidates) => {
        this.hairCandidates.set(candidates);
      },
      error: (error) => {
        console.error("Failed to load hair candidates:", error);
      },
    });

    this.styleCatalogService.getBeardStyles().subscribe({
      next: (styles) => {
        this.beardStyles.set(styles);
      },
      error: (error) => {
        console.error("Failed to load beard styles:", error);
      },
    });
    this.consultationService.getBeardCandidates(consultationId).subscribe({
      next: (candidates) => {
        this.beardCandidates.set(candidates);
      },
      error: (error) => {
        console.error("Failed to load beard candidates:", error);
      },
    });
  }

  previousHairStyle() {
    const styles = this.hairStyles();

    if (styles.length === 0) {
      return;
    }

    this.hairStyleIndex.update((index) =>
      index === 0 ? styles.length - 1 : index - 1,
    );
  }

  nextHairStyle() {
    const styles = this.hairStyles();

    if (styles.length === 0) {
      return;
    }

    this.hairStyleIndex.update((index) =>
      index === styles.length - 1 ? 0 : index + 1,
    );
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
        next: () => {
          this.isUploading.set(false);
          this.uploadSuccess.set(true);
        },
        error: (error) => {
          console.error("Image upload failed:", error);
          this.isUploading.set(false);
        },
      });
  }

  hasHairCandidate(hairStyleId: number) {
    return this.hairCandidates().some(
      (candidate) => candidate.hairStyleId === hairStyleId,
    );
  }

  getHairCandidate(hairStyleId: number) {
    return this.hairCandidates().find(
      (candidate) => candidate.hairStyleId === hairStyleId,
    );
  }

  getHairStyleById(hairStyleId: number) {
    return this.hairStyles().find((style) => style.id === hairStyleId);
  }

  addHairCandidate(hairStyleId: number) {
    const consultation = this.consultation();

    if (
      !consultation ||
      this.hasHairCandidate(hairStyleId) ||
      this.hairCandidates().length >= this.maxHairCandidates
    ) {
      return;
    }

    this.consultationService
      .addHairCandidate(consultation.id, hairStyleId)
      .subscribe({
        next: (candidate) => {
          this.hairCandidates.update((candidates) => [
            ...candidates,
            candidate,
          ]);
        },
        error: (error) => {
          console.error("Failed to add hair candidate:", error);
        },
      });
  }

  selectHairCandidate(candidateId: number) {
    const consultation = this.consultation();

    if (!consultation) {
      return;
    }

    this.consultationService
      .selectHairCandidate(consultation.id, candidateId)
      .subscribe({
        next: (selectedCandidate) => {
          this.hairCandidates.update((candidates) =>
            candidates.map((candidate) => ({
              ...candidate,
              isSelected: candidate.id === selectedCandidate.id,
            })),
          );
        },
        error: (error) => {
          console.error("Failed to select hair candidate:", error);
        },
      });
  }

  removeHairCandidate(candidateId: number) {
    const consultation = this.consultation();

    if (!consultation) {
      return;
    }

    this.consultationService
      .removeHairCandidate(consultation.id, candidateId)
      .subscribe({
        next: () => {
          this.hairCandidates.update((candidates) =>
            candidates.filter((candidate) => candidate.id !== candidateId),
          );
        },
        error: (error) => {
          console.error("Failed to remove hair candidate:", error);
        },
      });
  }
  previousBeardStyle() {
    const styles = this.beardStyles();

    if (styles.length === 0) {
      return;
    }

    this.beardStyleIndex.update((index) =>
      index === 0 ? styles.length - 1 : index - 1,
    );
  }

  nextBeardStyle() {
    const styles = this.beardStyles();

    if (styles.length === 0) {
      return;
    }

    this.beardStyleIndex.update((index) =>
      index === styles.length - 1 ? 0 : index + 1,
    );
  }

  hasBeardCandidate(beardStyleId: number) {
    return this.beardCandidates().some(
      (candidate) => candidate.beardStyleId === beardStyleId,
    );
  }

  getBeardCandidate(beardStyleId: number) {
    return this.beardCandidates().find(
      (candidate) => candidate.beardStyleId === beardStyleId,
    );
  }

  getBeardStyleById(beardStyleId: number) {
    return this.beardStyles().find((style) => style.id === beardStyleId);
  }

  addBeardCandidate(beardStyleId: number) {
    const consultation = this.consultation();

    if (
      !consultation ||
      this.hasBeardCandidate(beardStyleId) ||
      this.beardCandidates().length >= this.maxBeardCandidates
    ) {
      return;
    }

    this.consultationService
      .addBeardCandidate(consultation.id, beardStyleId)
      .subscribe({
        next: (candidate) => {
          this.beardCandidates.update((candidates) => [
            ...candidates,
            candidate,
          ]);
        },
        error: (error) => {
          console.error("Failed to add beard candidate:", error);
        },
      });
  }

  selectBeardCandidate(candidateId: number) {
    const consultation = this.consultation();

    if (!consultation) {
      return;
    }

    this.consultationService
      .selectBeardCandidate(consultation.id, candidateId)
      .subscribe({
        next: (selectedCandidate) => {
          this.beardCandidates.update((candidates) =>
            candidates.map((candidate) => ({
              ...candidate,
              isSelected: candidate.id === selectedCandidate.id,
            })),
          );
        },
        error: (error) => {
          console.error("Failed to select beard candidate:", error);
        },
      });
  }

  removeBeardCandidate(candidateId: number) {
    const consultation = this.consultation();

    if (!consultation) {
      return;
    }

    this.consultationService
      .removeBeardCandidate(consultation.id, candidateId)
      .subscribe({
        next: () => {
          this.beardCandidates.update((candidates) =>
            candidates.filter((candidate) => candidate.id !== candidateId),
          );
        },
        error: (error) => {
          console.error("Failed to remove beard candidate:", error);
        },
      });
  }
}
