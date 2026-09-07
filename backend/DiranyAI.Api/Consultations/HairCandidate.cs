using DiranyAI.Api.Styles;

namespace DiranyAI.Api.Consultations;

public class HairCandidate
{
    public long Id { get; set; }

    public long ConsultationId { get; set; }
    public Consultation Consultation { get; set; } = null!;

    public long HairStyleId { get; set; }
    public HairStyle HairStyle { get; set; } = null!;

    public bool IsSelected { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}