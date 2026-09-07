using DiranyAI.Api.Common.Exceptions;
using DiranyAI.Api.Consultations.Dtos;
using DiranyAI.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Consultations;

public class HairCandidateService
{
    private readonly AppDbContext _dbContext;

    public HairCandidateService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HairCandidateResponse> AddHairCandidateAsync(
        long consultationId,
        AddHairCandidateRequest request,
        CancellationToken cancellationToken = default)
    {
        var consultationExists = await _dbContext.Consultations
            .AnyAsync(c => c.Id == consultationId, cancellationToken);

        if (!consultationExists)
        {
            throw new ConsultationNotFoundException(consultationId);
        }

        var styleExists = await _dbContext.HairStyles
            .AnyAsync(s => s.Id == request.HairStyleId && s.IsActive, cancellationToken);

        if (!styleExists)
        {
            throw new ArgumentException("The selected hair style does not exist or is inactive.");
        }

        var candidateExists = await _dbContext.HairCandidates
            .AnyAsync(c =>
                c.ConsultationId == consultationId &&
                c.HairStyleId == request.HairStyleId,
                cancellationToken);

        if (candidateExists)
        {
            throw new ArgumentException("This hair style has already been added to the consultation.");
        }
        var candidateCount = await _dbContext.HairCandidates
        .CountAsync(
        c => c.ConsultationId == consultationId,
        cancellationToken);

        if (candidateCount >= 3)
        {
            throw new ArgumentException(
                "A consultation can have a maximum of 3 hair candidates.");
        }

        var candidate = new HairCandidate
        {
            ConsultationId = consultationId,
            HairStyleId = request.HairStyleId,
            IsSelected = false,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _dbContext.HairCandidates.Add(candidate);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new HairCandidateResponse
        {
            Id = candidate.Id,
            ConsultationId = candidate.ConsultationId,
            HairStyleId = candidate.HairStyleId,
            IsSelected = candidate.IsSelected,
            CreatedAt = candidate.CreatedAt
        };
    }
    public async Task<HairCandidateResponse> SelectHairCandidateAsync(
    long consultationId,
    long candidateId,
    CancellationToken cancellationToken = default)
    {
        var candidate = await _dbContext.HairCandidates
            .FirstOrDefaultAsync(
                c => c.Id == candidateId &&
                     c.ConsultationId == consultationId,
                cancellationToken);

        if (candidate is null)
        {
            throw new ArgumentException(
                "The hair candidate does not exist for this consultation.");
        }

        var selectedCandidates = await _dbContext.HairCandidates
            .Where(c =>
                c.ConsultationId == consultationId &&
                c.IsSelected)
            .ToListAsync(cancellationToken);

        foreach (var selected in selectedCandidates)
        {
            selected.IsSelected = false;
        }

        candidate.IsSelected = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new HairCandidateResponse
        {
            Id = candidate.Id,
            ConsultationId = candidate.ConsultationId,
            HairStyleId = candidate.HairStyleId,
            IsSelected = candidate.IsSelected,
            CreatedAt = candidate.CreatedAt
        };
    }
    public async Task<List<HairCandidateResponse>> GetAllAsync(
     long consultationId,
     CancellationToken cancellationToken = default)
    {
        return await _dbContext.HairCandidates
            .Where(c => c.ConsultationId == consultationId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new HairCandidateResponse
            {
                Id = c.Id,
                ConsultationId = c.ConsultationId,
                HairStyleId = c.HairStyleId,
                IsSelected = c.IsSelected,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
    public async Task DeleteAsync(
    long consultationId,
    long candidateId,
    CancellationToken cancellationToken = default)
    {
        var candidate = await _dbContext.HairCandidates
            .FirstOrDefaultAsync(
                c => c.Id == candidateId &&
                     c.ConsultationId == consultationId,
                cancellationToken);

        if (candidate is null)
        {
            throw new ArgumentException(
                "The hair candidate does not exist for this consultation.");
        }

        _dbContext.HairCandidates.Remove(candidate);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}