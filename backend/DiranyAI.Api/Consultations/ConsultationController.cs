using DiranyAI.Api.Consultations.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DiranyAI.Api.Consultations;

[ApiController]
[Route("api/customers/{customerId:long}/consultations")]
public class ConsultationController : ControllerBase
{
    private readonly ConsultationService _consultationService;
    private readonly ConsultationImageService _consultationImageService;

    public ConsultationController(
        ConsultationService consultationService,
        ConsultationImageService consultationImageService)
    {
        _consultationService = consultationService;
        _consultationImageService = consultationImageService;
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
    [HttpPost("/api/consultations/{consultationId:long}/images/original")]
    public async Task<ActionResult<ConsultationImageResponse>> UploadOriginalImage(
    long consultationId,
    IFormFile file,
    CancellationToken cancellationToken)
    {
        var image = await _consultationImageService.UploadOriginalAsync(
            consultationId,
            file,
            cancellationToken);

        return Ok(image);
    }
}