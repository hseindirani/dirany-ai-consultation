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
}