using DiranyAI.Api.Common.Exceptions;
using DiranyAI.Api.Consultations.Dtos;
using DiranyAI.Api.Data;
using DiranyAI.Api.Storage;
using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Consultations;

public class ConsultationImageService
{
    private readonly AppDbContext _dbContext;
    private readonly IImageStorage _imageStorage;

    public ConsultationImageService(
        AppDbContext dbContext,
        IImageStorage imageStorage)
    {
        _dbContext = dbContext;
        _imageStorage = imageStorage;
    }

    public async Task<ConsultationImageResponse> UploadOriginalAsync(
        long consultationId,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
        {
            throw new ArgumentException("The uploaded image is empty.");
        }
        var allowedContentTypes = new[]
           {
                    "image/jpeg",
                    "image/png",
                    "image/webp"
           };

        if (!allowedContentTypes.Contains(file.ContentType))
        {
            throw new ArgumentException("Only JPEG, PNG, and WEBP images are allowed.");
        }
        const long maxFileSize = 10 * 1024 * 1024;

        if (file.Length > maxFileSize)
        {
            throw new ArgumentException("The uploaded image must be 10 MB or smaller.");
        }
        var consultationExists = await _dbContext.Consultations
            .AnyAsync(c => c.Id == consultationId, cancellationToken);

        if (!consultationExists)
        {
            throw new ConsultationNotFoundException(consultationId);
        }

        var storagePath = await _imageStorage.SaveAsync(
            file.OpenReadStream(),
            file.FileName,
            cancellationToken);

        var image = new ConsultationImage
        {
            ConsultationId = consultationId,
            ImageType = ConsultationImageType.Original,
            StoragePath = storagePath,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _dbContext.ConsultationImages.Add(image);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ConsultationImageResponse
        {
            Id = image.Id,
            ConsultationId = image.ConsultationId,
            ImageType = image.ImageType,
            
            CreatedAt = image.CreatedAt
        };
    }
    public async Task<(Stream Stream, string ContentType)> GetImageAsync(
    long consultationId,
    long imageId,
    CancellationToken cancellationToken = default)
    {
        var image = await _dbContext.ConsultationImages
            .FirstOrDefaultAsync(
                i => i.Id == imageId &&
                     i.ConsultationId == consultationId,
                cancellationToken);

        if (image is null)
        {
            throw new KeyNotFoundException("The consultation image does not exist.");
        }

        var stream = await _imageStorage.OpenReadAsync(
            image.StoragePath,
            cancellationToken);

        var contentType = Path.GetExtension(image.StoragePath).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };

        return (stream, contentType);
    }
    public async Task<List<ConsultationImageResponse>> GetImagesAsync(
    long consultationId,
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.ConsultationImages
            .Where(i => i.ConsultationId == consultationId)
            .OrderBy(i => i.CreatedAt)
            .Select(i => new ConsultationImageResponse
            {
                Id = i.Id,
                ConsultationId = i.ConsultationId,
                HairCandidateId = i.HairCandidateId,
                ImageType = i.ImageType,
               
                CreatedAt = i.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}