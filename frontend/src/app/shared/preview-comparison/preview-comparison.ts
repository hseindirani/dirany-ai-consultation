import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
  selector: "app-preview-comparison",
  imports: [],
  templateUrl: "./preview-comparison.html",
  styleUrl: "./preview-comparison.css",
})
export class PreviewComparison {
  @Input() referenceUrl: string | null = null;
  @Input() generatedUrl: string | null = null;

  @Input() generatedTitle = "Preview";
  @Input() generatedAlt = "Generated preview";

  @Input() styleName: string | null = null;

  @Input() emptyMessage = "Generate a preview using the selected style.";
  @Input() referenceMissingMessage = "Upload an original photo first.";

  @Input() generateLabel = "Generate AI Preview";
  @Input() generatingLabel = "Generating...";
  @Input() regenerateLabel = "Generate again";

  @Input() isGenerating = false;
  @Input() error: string | null = null;

  @Output() generate = new EventEmitter<void>();

  onGenerate() {
    if (!this.referenceUrl || this.isGenerating) {
      return;
    }

    this.generate.emit();
  }
}