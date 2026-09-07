using System.ComponentModel.DataAnnotations;

namespace DiranyAI.Api.Consultations.Dtos;

public class AddHairCandidateRequest
{
    [Required]
    public long HairStyleId { get; set; }
}