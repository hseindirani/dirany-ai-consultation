namespace DiranyAI.Api.Consultations.Dtos;

public class ConsultationResponse
{
    public long Id { get; set; }

    public long CustomerId { get; set; }

    public string? Notes { get; set; }

    public ConsultationStatus Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }
}