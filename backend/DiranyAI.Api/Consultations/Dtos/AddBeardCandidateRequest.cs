using System.ComponentModel.DataAnnotations;

namespace DiranyAI.Api.Consultations.Dtos;

public class AddBeardCandidateRequest
{
    [Required]
    public long BeardStyleId { get; set; }
}