using DiranyAI.Api.Common.Exceptions;
using DiranyAI.Api.Consultations.Dtos;
using DiranyAI.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Consultations;

public class BeardCandidateService
{
    private readonly AppDbContext _dbContext;

    public BeardCandidateService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BeardCandidateResponse> AddBeardCandidateAsync(
        long consultationId,
        AddBeardCandidateRequest request,
        CancellationToken cancellationToken = default)
    {
        var consultationExists = await _dbContext.Consultations
            .AnyAsync(c => c.Id == consultationId, cancellationToken);

        if (!consultationExists)
        {
            throw new ConsultationNotFoundException(consultationId);
        }

        var styleExists = await _dbContext.BeardStyles
            .AnyAsync(
                s => s.Id == request.BeardStyleId && s.IsActive,
                cancellationToken);

        if (!styleExists)
        {
            throw new ArgumentException(
                "The selected beard style does not exist or is inactive.");
        }

        var candidateExists = await _dbContext.BeardCandidates
            .AnyAsync(
                c => c.ConsultationId == consultationId &&
                     c.BeardStyleId == request.BeardStyleId,
                cancellationToken);

        if (candidateExists)
        {
            throw new ArgumentException(
                "This beard style has already been added to the consultation.");
        }

        var candidate = new BeardCandidate
        {
            ConsultationId = consultationId,
            BeardStyleId = request.BeardStyleId,
            IsSelected = false,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _dbContext.BeardCandidates.Add(candidate);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new BeardCandidateResponse
        {
            Id = candidate.Id,
            ConsultationId = candidate.ConsultationId,
            BeardStyleId = candidate.BeardStyleId,
            IsSelected = candidate.IsSelected,
            CreatedAt = candidate.CreatedAt
        };
    }
}