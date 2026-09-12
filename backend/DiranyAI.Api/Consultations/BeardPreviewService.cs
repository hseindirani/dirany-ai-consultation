using DiranyAI.Api.AI;
using DiranyAI.Api.Consultations.Dtos;
using DiranyAI.Api.Data;
using DiranyAI.Api.Storage;
using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Consultations;

public class BeardPreviewService
{
    private readonly AppDbContext _dbContext;
    private readonly IImageStorage _imageStorage;
    private readonly IImageGenerationService _imageGenerationService;

    public BeardPreviewService(
        AppDbContext dbContext,
        IImageStorage imageStorage,
        IImageGenerationService imageGenerationService)
    {
        _dbContext = dbContext;
        _imageStorage = imageStorage;
        _imageGenerationService = imageGenerationService;
    }

    public async Task<BeardPreviewResponse> GenerateAsync(
        long consultationId,
        long candidateId,
        CancellationToken cancellationToken = default)
    {
        var candidate = await _dbContext.BeardCandidates
            .Include(c => c.BeardStyle)
            .FirstOrDefaultAsync(
                c => c.Id == candidateId &&
                     c.ConsultationId == consultationId,
                cancellationToken);

        if (candidate is null)
        {
            throw new ArgumentException(
                "The beard candidate does not exist for this consultation.");
        }

        var originalImage = await _dbContext.ConsultationImages
            .FirstOrDefaultAsync(
                i => i.ConsultationId == consultationId &&
                     i.ImageType == ConsultationImageType.Original,
                cancellationToken);

        if (originalImage is null)
        {
            throw new ArgumentException(
                "This consultation does not have an original image.");
        }

        await using var originalImageStream =
            await _imageStorage.OpenReadAsync(
                originalImage.StoragePath,
                cancellationToken);

        var prompt =
            $"Edit the beard to: {candidate.BeardStyle.AiPromptHint}. " +
            "Keep the same person, facial features, skin tone, hairstyle, pose, lighting, and background. " +
            "Change only the beard and make the result photorealistic.";

        var fileName = Path.GetFileName(originalImage.StoragePath);

        var contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => throw new ArgumentException("Unsupported original image format.")
        };

        var imageInput = new ImageInput
        {
            Stream = originalImageStream,
            FileName = fileName,
            ContentType = contentType
        };

        await using var generatedImageStream =
            await _imageGenerationService.GenerateBeardPreviewAsync(
                imageInput,
                prompt,
                cancellationToken);

        var previewFileName =
            $"beard-preview-{consultationId}-{candidateId}.png";

        var storagePath =
            await _imageStorage.SaveAsync(
                generatedImageStream,
                previewFileName,
                cancellationToken);

        var previewImage = new ConsultationImage
        {
            ConsultationId = consultationId,
            BeardCandidateId = candidateId,
            ImageType = ConsultationImageType.BeardPreview,
            StoragePath = storagePath,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _dbContext.ConsultationImages.Add(previewImage);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new BeardPreviewResponse
        {
            ImageId = previewImage.Id,
            BeardCandidateId = candidateId
        };
    }
}