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
}