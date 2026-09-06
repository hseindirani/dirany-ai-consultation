using System.ComponentModel.DataAnnotations;

namespace DiranyAI.Api.Consultations.Dtos;

public class CreateConsultationRequest
{
    [MaxLength(1000)]
    public string? Notes { get; set; }
}