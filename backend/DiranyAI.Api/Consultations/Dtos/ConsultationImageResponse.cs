namespace DiranyAI.Api.Consultations.Dtos;

public class ConsultationImageResponse
{
    public long Id { get; set; }
    public long ConsultationId { get; set; }
    public ConsultationImageType ImageType { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}