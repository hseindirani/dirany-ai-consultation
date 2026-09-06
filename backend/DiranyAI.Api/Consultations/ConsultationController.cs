using DiranyAI.Api.Consultations.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DiranyAI.Api.Consultations;

[ApiController]
[Route("api/customers/{customerId:long}/consultations")]
public class ConsultationController : ControllerBase
{
    private readonly ConsultationService _consultationService;

    public ConsultationController(ConsultationService consultationService)
    {
        _consultationService = consultationService;
    }

    [HttpPost]
    public async Task<ActionResult<ConsultationResponse>> CreateConsultation(
        long customerId,
        CreateConsultationRequest request)
    {
        var consultation = await _consultationService
            .CreateConsultationAsync(customerId, request);

        return CreatedAtAction(
                   nameof(GetConsultation),
                   new { id = consultation.Id },
                   consultation);
    }

    [HttpGet("/api/consultations/{id:long}")]
    public async Task<ActionResult<ConsultationResponse>> GetConsultation(long id)
    {
        var consultation = await _consultationService
            .GetConsultationByIdAsync(id);

        return Ok(consultation);
    }
}