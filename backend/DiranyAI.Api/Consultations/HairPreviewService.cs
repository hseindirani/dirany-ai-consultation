using DiranyAI.Api.AI;
using DiranyAI.Api.Data;
using DiranyAI.Api.Storage;
using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Consultations;

public class HairPreviewService
{
    private readonly AppDbContext _dbContext;
    private readonly IImageStorage _imageStorage;
    private readonly IImageGenerationService _imageGenerationService;

    public HairPreviewService(
        AppDbContext dbContext,
        IImageStorage imageStorage,
        IImageGenerationService imageGenerationService)
    {
        _dbContext = dbContext;
        _imageStorage = imageStorage;
        _imageGenerationService = imageGenerationService;
    }
    public async Task GenerateAsync(
    long consultationId,
    long candidateId,
    CancellationToken cancellationToken = default)
    {
        var candidate = await _dbContext.HairCandidates
            .Include(c => c.HairStyle)
            .FirstOrDefaultAsync(
                c => c.Id == candidateId &&
                     c.ConsultationId == consultationId,
                cancellationToken);

        if (candidate is null)
        {
            throw new ArgumentException(
                "The hair candidate does not exist for this consultation.");
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
    $"Edit the hairstyle to: {candidate.HairStyle.AiPromptHint}. " +
    "Keep the same person, facial features, skin tone, pose, lighting, and background. " +
    "Change only the hairstyle and make the result photorealistic.";
    }
}