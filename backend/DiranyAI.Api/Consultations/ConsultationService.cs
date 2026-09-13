using DiranyAI.Api.Common.Exceptions;
using DiranyAI.Api.Consultations.Dtos;
using DiranyAI.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Consultations;

public class ConsultationService
{
    private readonly AppDbContext _dbContext;

    public ConsultationService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ConsultationResponse> CreateConsultationAsync(
        long customerId,
        CreateConsultationRequest request)
    {
        var customerExists = await _dbContext.Customers
            .AnyAsync(c => c.Id == customerId);

        if (!customerExists)
        {
            throw new CustomerNotFoundException(customerId);
        }

        var consultation = new Consultation
        {
            CustomerId = customerId,
            Notes = request.Notes?.Trim(),
            Status = ConsultationStatus.InProgress,
            CreatedAt = DateTimeOffset.UtcNow,
            CompletedAt = null
        };

        _dbContext.Consultations.Add(consultation);

        await _dbContext.SaveChangesAsync();

        return new ConsultationResponse
        {
            Id = consultation.Id,
            CustomerId = consultation.CustomerId,
            Notes = consultation.Notes,
            Status = consultation.Status,
            CreatedAt = consultation.CreatedAt,
            CompletedAt = consultation.CompletedAt
        };
    }
    public async Task<ConsultationResponse> GetConsultationByIdAsync(long id)
    {
        var consultation = await _dbContext.Consultations
            .FirstOrDefaultAsync(c => c.Id == id);

        if (consultation is null)
        {
            throw new ConsultationNotFoundException(id);
        }

        return new ConsultationResponse
        {
            Id = consultation.Id,
            CustomerId = consultation.CustomerId,
            Notes = consultation.Notes,
            Status = consultation.Status,
            CreatedAt = consultation.CreatedAt,
            CompletedAt = consultation.CompletedAt
        };
    }
    public async Task<ConsultationResponse> UpdateNotesAsync(
    long consultationId,
    UpdateConsultationNotesRequest request,
    CancellationToken cancellationToken = default)
    {
        var consultation = await _dbContext.Consultations
            .FirstOrDefaultAsync(
                c => c.Id == consultationId,
                cancellationToken);

        if (consultation is null)
        {
            throw new ConsultationNotFoundException(consultationId);
        }

        if (consultation.Status == ConsultationStatus.Completed)
        {
            throw new ArgumentException(
                "A completed consultation cannot be modified.");
        }

        consultation.Notes = request.Notes?.Trim();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ConsultationResponse
        {
            Id = consultation.Id,
            CustomerId = consultation.CustomerId,
            Notes = consultation.Notes,
            Status = consultation.Status,
            CreatedAt = consultation.CreatedAt,
            CompletedAt = consultation.CompletedAt
        };
    }
    public async Task<ConsultationResponse> CompleteAsync(
    long consultationId,
    CancellationToken cancellationToken = default)
    {
        var consultation = await _dbContext.Consultations
            .FirstOrDefaultAsync(
                c => c.Id == consultationId,
                cancellationToken);

        if (consultation is null)
        {
            throw new ConsultationNotFoundException(consultationId);
        }

        if (consultation.Status == ConsultationStatus.Completed)
        {
            throw new ArgumentException(
                "This consultation is already completed.");
        }

        var hasFinalResult = await _dbContext.ConsultationImages
            .AnyAsync(
                i => i.ConsultationId == consultationId &&
                     i.ImageType == ConsultationImageType.FinalResult,
                cancellationToken);

        if (!hasFinalResult)
        {
            throw new ArgumentException(
                "A final result image must be uploaded before completing the consultation.");
        }

        consultation.Status = ConsultationStatus.Completed;
        consultation.CompletedAt = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ConsultationResponse
        {
            Id = consultation.Id,
            CustomerId = consultation.CustomerId,
            Notes = consultation.Notes,
            Status = consultation.Status,
            CreatedAt = consultation.CreatedAt,
            CompletedAt = consultation.CompletedAt
        };
    }
}