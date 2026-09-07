using DiranyAI.Api.Customers;

namespace DiranyAI.Api.Consultations;

public class Consultation
{
    public long Id { get; set; }

    public long CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public string? Notes { get; set; }

    public ConsultationStatus Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public ICollection<ConsultationImage> Images { get; set; }
    = new List<ConsultationImage>();
    public ICollection<HairCandidate> HairCandidates { get; set; }
    = new List<HairCandidate>();

    public ICollection<BeardCandidate> BeardCandidates { get; set; }
        = new List<BeardCandidate>();
}