namespace DiranyAI.Api.Consultations;

public class ConsultationImage
{
    public long Id { get; set; }

    public long ConsultationId { get; set; }
    public Consultation Consultation { get; set; } = null!;

    public long? HairCandidateId { get; set; }
    public HairCandidate? HairCandidate { get; set; }

    public ConsultationImageType ImageType { get; set; }

    public string StoragePath { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}