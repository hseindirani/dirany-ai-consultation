namespace DiranyAI.Api.Consultations.Dtos;

public class ConsultationImageResponse
{
    public long Id { get; set; }
    public long ConsultationId { get; set; }
    public long? HairCandidateId { get; set; }
    public ConsultationImageType ImageType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}