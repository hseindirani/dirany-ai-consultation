import { Component, OnInit, computed, signal } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import {
  BeardCandidate,
  Consultation,
  ConsultationImage,
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
  consultationImages = signal<ConsultationImage[]>([]);
  isGeneratingHairPreview = signal(false);
  hairPreviewError = signal<string | null>(null);
  selectedBeardPreviewUrl = signal<string | null>(null);
  selectedBeardPreviewImageId = signal<number | null>(null);
  isGeneratingBeardPreview = signal(false);
  beardPreviewError = signal<string | null>(null);

  readonly maxHairCandidates = 3;
  readonly maxBeardCandidates = 3;
  selectedHairPreviewUrl = signal<string | null>(null);
  selectedHairPreviewImageId = signal<number | null>(null);

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
        this.loadConsultationImages(consultationId);
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

        const consultation = this.consultation();

        if (consultation) {
          this.loadConsultationImages(consultation.id);
        }
      },
      error: (error) => {
        console.error("Failed to load beard candidates:", error);
      },
    });
    this.loadConsultationImages(consultationId);
  }
  loadConsultationImages(consultationId: number) {
    this.consultationService.getImages(consultationId).subscribe({
      next: (images) => {
        this.consultationImages.set(images);

        const originalImage = images.find(
          (image) => image.imageType === "Original",
        );

        if (originalImage) {
          this.previewUrl = this.consultationService.getImageUrl(
            consultationId,
            originalImage.id,
          );
        }

        const selectedHairCandidate = this.hairCandidates().find(
          (candidate) => candidate.isSelected,
        );

        if (!selectedHairCandidate) {
          this.selectedHairPreviewUrl.set(null);
          this.selectedHairPreviewImageId.set(null);
        } else {
          const hairPreview = images
            .filter(
              (image) =>
                image.imageType === "HairPreview" &&
                image.hairCandidateId === selectedHairCandidate.id,
            )
            .sort((a, b) => {
              const createdAtDifference =
                new Date(b.createdAt).getTime() -
                new Date(a.createdAt).getTime();

              if (createdAtDifference !== 0) {
                return createdAtDifference;
              }

              return b.id - a.id;
            })[0];

          if (hairPreview) {
            this.selectedHairPreviewImageId.set(hairPreview.id);

            this.selectedHairPreviewUrl.set(
              this.consultationService.getImageUrl(
                consultationId,
                hairPreview.id,
              ),
            );
          } else {
            this.selectedHairPreviewUrl.set(null);
            this.selectedHairPreviewImageId.set(null);
          }
        }
        const selectedBeardCandidate = this.beardCandidates().find(
          (candidate) => candidate.isSelected,
        );

        if (!selectedBeardCandidate) {
          this.selectedBeardPreviewUrl.set(null);
          this.selectedBeardPreviewImageId.set(null);
          return;
        }

        const beardPreview = images
          .filter(
            (image) =>
              image.imageType === "BeardPreview" &&
              image.beardCandidateId === selectedBeardCandidate.id,
          )
          .sort((a, b) => {
            const createdAtDifference =
              new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();

            if (createdAtDifference !== 0) {
              return createdAtDifference;
            }

            return b.id - a.id;
          })[0];

        if (beardPreview) {
          this.selectedBeardPreviewImageId.set(beardPreview.id);

          this.selectedBeardPreviewUrl.set(
            this.consultationService.getImageUrl(
              consultationId,
              beardPreview.id,
            ),
          );
        } else {
          this.selectedBeardPreviewUrl.set(null);
          this.selectedBeardPreviewImageId.set(null);
        }
      },
      error: (error) => {
        console.error("Failed to load consultation images:", error);
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

    if (this.previewUrl?.startsWith("blob:")) {
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

          this.loadConsultationImages(consultation.id);
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

          this.selectedHairPreviewUrl.set(null);
          this.selectedHairPreviewImageId.set(null);

          this.loadConsultationImages(consultation.id);
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

          this.selectedBeardPreviewUrl.set(null);
          this.selectedBeardPreviewImageId.set(null);

          this.loadConsultationImages(consultation.id);
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
  getHairStyleImage(styleName: string): string {
    const images: Record<string, string> = {
      "Taper Fade": "/images/styles/hair/taper-fade.png",
      "Buzz Cut": "/images/styles/hair/buzz-cut.png",
      Mohawk: "/images/styles/hair/mohawk.png",
      "Textured Crop": "/images/styles/hair/textured-crop.png",
      "Slick Back": "/images/styles/hair/slick-back.png",
    };

    return images[styleName] ?? "";
  }

  getBeardStyleImage(styleName: string): string {
    const images: Record<string, string> = {
      "Short Boxed Beard": "/images/styles/beard/short-boxed-beard.png",
      "Pointy Beard": "/images/styles/beard/pointy-beard.png",
      "Italian Beard": "/images/styles/beard/italian-beard.png",
      Stubble: "/images/styles/beard/stubble.png",
      "Full Beard": "/images/styles/beard/full-beard.png",
    };

    return images[styleName] ?? "";
  }
  generateSelectedHairPreview() {
    const consultation = this.consultation();

    const selectedCandidate = this.hairCandidates().find(
      (candidate) => candidate.isSelected,
    );

    if (!consultation || !selectedCandidate || this.isGeneratingHairPreview()) {
      return;
    }

    this.isGeneratingHairPreview.set(true);
    this.hairPreviewError.set(null);

    this.consultationService
      .generateHairPreview(consultation.id, selectedCandidate.id)
      .subscribe({
        next: (response) => {
          this.selectedHairPreviewImageId.set(response.imageId);

          this.selectedHairPreviewUrl.set(
            this.consultationService.getImageUrl(
              consultation.id,
              response.imageId,
            ),
          );

          this.isGeneratingHairPreview.set(false);

          this.loadConsultationImages(consultation.id);
        },
        error: (error) => {
          console.error("Failed to generate hair preview:", error);

          this.hairPreviewError.set(
            "Could not generate the preview. Please try again.",
          );

          this.isGeneratingHairPreview.set(false);
        },
      });
  }
  generateSelectedBeardPreview() {
    const consultation = this.consultation();

    const selectedCandidate = this.beardCandidates().find(
      (candidate) => candidate.isSelected,
    );

    if (
      !consultation ||
      !selectedCandidate ||
      this.isGeneratingBeardPreview()
    ) {
      return;
    }

    this.isGeneratingBeardPreview.set(true);
    this.beardPreviewError.set(null);

    this.consultationService
      .generateBeardPreview(consultation.id, selectedCandidate.id)
      .subscribe({
        next: (response) => {
          this.selectedBeardPreviewImageId.set(response.imageId);

          this.selectedBeardPreviewUrl.set(
            this.consultationService.getImageUrl(
              consultation.id,
              response.imageId,
            ),
          );

          this.isGeneratingBeardPreview.set(false);

          this.loadConsultationImages(consultation.id);
        },
        error: (error) => {
          console.error("Failed to generate beard preview:", error);

          this.beardPreviewError.set(
            "Could not generate the preview. Please try again.",
          );

          this.isGeneratingBeardPreview.set(false);
        },
      });
  }
  getSelectedHairCandidate() {
    return this.hairCandidates().find((candidate) => candidate.isSelected);
  }

  getSelectedHairStyle() {
    const candidate = this.getSelectedHairCandidate();

    if (!candidate) {
      return null;
    }

    return this.getHairStyleById(candidate.hairStyleId) ?? null;
  }
  getSelectedBeardCandidate() {
    return this.beardCandidates().find((candidate) => candidate.isSelected);
  }

  getSelectedBeardStyle() {
    const candidate = this.getSelectedBeardCandidate();

    if (!candidate) {
      return null;
    }

    return this.getBeardStyleById(candidate.beardStyleId) ?? null;
  }
}
