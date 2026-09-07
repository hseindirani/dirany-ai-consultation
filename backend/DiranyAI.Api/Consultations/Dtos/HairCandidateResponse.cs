namespace DiranyAI.Api.Consultations.Dtos;

public class HairCandidateResponse
{
    public long Id { get; set; }
    public long ConsultationId { get; set; }
    public long HairStyleId { get; set; }
    public bool IsSelected { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}