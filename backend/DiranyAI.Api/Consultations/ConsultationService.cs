using DiranyAI.Api.Common.Exceptions;
using DiranyAI.Api.Consultations.Dtos;
using DiranyAI.Api.Data;
using Microsoft.EntityFrameworkCore;
using DiranyAI.Api.Customers;

namespace DiranyAI.Api.Consultations;

public class ConsultationService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ConsultationService> _logger;

    public ConsultationService(
        AppDbContext dbContext,
        ILogger<ConsultationService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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
        _logger.LogInformation(
               "Consultation {ConsultationId} completed for customer {CustomerId}",
                consultation.Id,
                consultation.CustomerId);

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
    public async Task<List<ConsultationResponse>> GetCustomerConsultationsAsync(
    long customerId,
    CancellationToken cancellationToken = default)
    {
        var customerExists = await _dbContext.Customers
            .AnyAsync(c => c.Id == customerId, cancellationToken);

        if (!customerExists)
        {
            throw new CustomerNotFoundException(customerId);
        }

        return await _dbContext.Consultations
            .Where(c => c.CustomerId == customerId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new ConsultationResponse
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                Notes = c.Notes,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                CompletedAt = c.CompletedAt
            })
            .ToListAsync(cancellationToken);
    }
}