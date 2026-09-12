import { Component, OnInit, computed, signal } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { PreviewComparison } from "../../../shared/preview-comparison/preview-comparison";

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
  imports: [PreviewComparison],
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
  sideSelectedFile = signal<File | null>(null);
  sideSelectedFileName = signal<string | null>(null);
  isUploadingSidePhoto = signal(false);
  sideUploadSuccess = signal(false);
  sidePreviewUrl = signal<string | null>(null);
  isGeneratingHairPreview = signal(false);
  hairPreviewError = signal<string | null>(null);
  selectedBeardPreviewUrl = signal<string | null>(null);
  selectedBeardPreviewImageId = signal<number | null>(null);
  isGeneratingBeardPreview = signal(false);
  beardPreviewError = signal<string | null>(null);
  selectedCombinedPreviewUrl = signal<string | null>(null);
  selectedCombinedPreviewImageId = signal<number | null>(null);
  isGeneratingCombinedPreview = signal(false);
  combinedPreviewError = signal<string | null>(null);
  activePreviewMode = signal<"hair" | "beard" | "combined">("hair");
  activeHairAngle = signal<"Front" | "Side">("Front");

  readonly maxHairCandidates = 3;
  readonly maxBeardCandidates = 3;
  selectedHairPreviewUrl = signal<string | null>(null);
  selectedHairPreviewImageId = signal<number | null>(null);
  selectedSideHairPreviewUrl = signal<string | null>(null);
  selectedSideHairPreviewImageId = signal<number | null>(null);
  isGeneratingSideHairPreview = signal(false);
  sideHairPreviewError = signal<string | null>(null);

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

        // ORIGINAL IMAGE
        const originalImage = images
          .filter(
            (image) =>
              image.imageType === "Original" && image.imageAngle === "Front",
          )
          .sort((a, b) => {
            const createdAtDifference =
              new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();

            if (createdAtDifference !== 0) {
              return createdAtDifference;
            }

            return b.id - a.id;
          })[0];

        if (originalImage) {
          this.previewUrl = this.consultationService.getImageUrl(
            consultationId,
            originalImage.id,
          );
        }
        const sideOriginalImage = images
          .filter(
            (image) =>
              image.imageType === "Original" && image.imageAngle === "Side",
          )
          .sort((a, b) => {
            const createdAtDifference =
              new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();

            if (createdAtDifference !== 0) {
              return createdAtDifference;
            }

            return b.id - a.id;
          })[0];

        if (sideOriginalImage) {
          this.sidePreviewUrl.set(
            this.consultationService.getImageUrl(
              consultationId,
              sideOriginalImage.id,
            ),
          );
        } else {
          this.sidePreviewUrl.set(null);
        }

        // CURRENTLY SELECTED CANDIDATES
        const selectedHairCandidate = this.hairCandidates().find(
          (candidate) => candidate.isSelected,
        );

        const selectedBeardCandidate = this.beardCandidates().find(
          (candidate) => candidate.isSelected,
        );

        // HAIR PREVIEW
        // HAIR PREVIEW
        if (!selectedHairCandidate) {
          this.selectedHairPreviewUrl.set(null);
          this.selectedHairPreviewImageId.set(null);

          this.selectedSideHairPreviewUrl.set(null);
          this.selectedSideHairPreviewImageId.set(null);
        } else {
          // FRONT
          const hairPreview = images
            .filter(
              (image) =>
                image.imageType === "HairPreview" &&
                image.imageAngle === "Front" &&
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

          // SIDE
          const sideHairPreview = images
            .filter(
              (image) =>
                image.imageType === "HairPreview" &&
                image.imageAngle === "Side" &&
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

          if (sideHairPreview) {
            this.selectedSideHairPreviewImageId.set(sideHairPreview.id);
            this.selectedSideHairPreviewUrl.set(
              this.consultationService.getImageUrl(
                consultationId,
                sideHairPreview.id,
              ),
            );
          } else {
            this.selectedSideHairPreviewUrl.set(null);
            this.selectedSideHairPreviewImageId.set(null);
          }
        }

        // BEARD PREVIEW
        if (!selectedBeardCandidate) {
          this.selectedBeardPreviewUrl.set(null);
          this.selectedBeardPreviewImageId.set(null);
        } else {
          const beardPreview = images
            .filter(
              (image) =>
                image.imageType === "BeardPreview" &&
                image.imageAngle === "Front" &&
                image.beardCandidateId === selectedBeardCandidate.id,
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
        }

        // COMBINED PREVIEW
        const combinedPreview =
          selectedHairCandidate && selectedBeardCandidate
            ? images
                .filter(
                  (image) =>
                    image.imageType === "CombinedPreview" &&
                    image.imageAngle === "Front" &&
                    image.hairCandidateId === selectedHairCandidate.id &&
                    image.beardCandidateId === selectedBeardCandidate.id,
                )
                .sort((a, b) => {
                  const createdAtDifference =
                    new Date(b.createdAt).getTime() -
                    new Date(a.createdAt).getTime();

                  if (createdAtDifference !== 0) {
                    return createdAtDifference;
                  }

                  return b.id - a.id;
                })[0]
            : undefined;

        if (combinedPreview) {
          this.selectedCombinedPreviewImageId.set(combinedPreview.id);

          this.selectedCombinedPreviewUrl.set(
            this.consultationService.getImageUrl(
              consultationId,
              combinedPreview.id,
            ),
          );
        } else {
          this.selectedCombinedPreviewImageId.set(null);
          this.selectedCombinedPreviewUrl.set(null);
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
  setPreviewMode(mode: "hair" | "beard" | "combined") {
    this.activePreviewMode.set(mode);
  }
  setHairAngle(angle: "Front" | "Side") {
    this.activeHairAngle.set(angle);
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
  onSideFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      this.sideSelectedFile.set(null);
      this.sideSelectedFileName.set(null);
      return;
    }

    this.sideSelectedFile.set(file);
    this.sideSelectedFileName.set(file.name);
    this.sideUploadSuccess.set(false);
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
  uploadSidePhoto() {
    const consultation = this.consultation();
    const file = this.sideSelectedFile();

    if (!consultation || !file || this.isUploadingSidePhoto()) {
      return;
    }

    this.isUploadingSidePhoto.set(true);
    this.sideUploadSuccess.set(false);

    this.consultationService
      .uploadSideOriginalPhoto(consultation.id, file)
      .subscribe({
        next: () => {
          this.isUploadingSidePhoto.set(false);
          this.sideUploadSuccess.set(true);

          this.sideSelectedFile.set(null);
          this.sideSelectedFileName.set(null);

          this.loadConsultationImages(consultation.id);
        },
        error: (error) => {
          console.error("Failed to upload side photo:", error);
          this.isUploadingSidePhoto.set(false);
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

          this.selectedSideHairPreviewUrl.set(null);
          this.selectedSideHairPreviewImageId.set(null);
          this.sideHairPreviewError.set(null);

          this.selectedCombinedPreviewUrl.set(null);
          this.selectedCombinedPreviewImageId.set(null);

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

          this.selectedCombinedPreviewUrl.set(null);
          this.selectedCombinedPreviewImageId.set(null);

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
  generateSelectedSideHairPreview() {
    const consultation = this.consultation();

    const selectedHairCandidate = this.hairCandidates().find(
      (candidate) => candidate.isSelected,
    );

    if (
      !consultation ||
      !selectedHairCandidate ||
      !this.sidePreviewUrl() ||
      this.isGeneratingSideHairPreview()
    ) {
      return;
    }

    this.isGeneratingSideHairPreview.set(true);
    this.sideHairPreviewError.set(null);

    this.consultationService
      .generateSideHairPreview(consultation.id, selectedHairCandidate.id)
      .subscribe({
        next: (response) => {
          this.selectedSideHairPreviewImageId.set(response.imageId);

          this.selectedSideHairPreviewUrl.set(
            this.consultationService.getImageUrl(
              consultation.id,
              response.imageId,
            ),
          );

          this.isGeneratingSideHairPreview.set(false);

          this.loadConsultationImages(consultation.id);
        },

        error: (error) => {
          console.error("Failed to generate side hair preview:", error);

          this.sideHairPreviewError.set(
            "Could not generate the side hair preview. Please try again.",
          );

          this.isGeneratingSideHairPreview.set(false);
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
  generateCombinedPreview() {
    const consultation = this.consultation();

    const selectedHairCandidate = this.hairCandidates().find(
      (candidate) => candidate.isSelected,
    );

    const selectedBeardCandidate = this.beardCandidates().find(
      (candidate) => candidate.isSelected,
    );

    if (
      !consultation ||
      !selectedHairCandidate ||
      !selectedBeardCandidate ||
      this.isGeneratingCombinedPreview()
    ) {
      return;
    }

    this.isGeneratingCombinedPreview.set(true);
    this.combinedPreviewError.set(null);

    this.consultationService
      .generateCombinedPreview(consultation.id)
      .subscribe({
        next: (response) => {
          this.selectedCombinedPreviewImageId.set(response.imageId);

          this.selectedCombinedPreviewUrl.set(
            this.consultationService.getImageUrl(
              consultation.id,
              response.imageId,
            ),
          );

          this.isGeneratingCombinedPreview.set(false);

          this.loadConsultationImages(consultation.id);
        },
        error: (error) => {
          console.error("Failed to generate combined preview:", error);

          this.combinedPreviewError.set(
            "Could not generate the combined preview. Please try again.",
          );

          this.isGeneratingCombinedPreview.set(false);
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
