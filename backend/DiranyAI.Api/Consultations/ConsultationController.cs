using DiranyAI.Api.Consultations.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DiranyAI.Api.Consultations;

[ApiController]
[Route("api/customers/{customerId:long}/consultations")]
public class ConsultationController : ControllerBase
{
    private readonly ConsultationService _consultationService;
    private readonly ConsultationImageService _consultationImageService;
    private readonly HairCandidateService _hairCandidateService;
    private readonly BeardCandidateService _beardCandidateService;
    private readonly HairPreviewService _hairPreviewService;

    public ConsultationController(
        ConsultationService consultationService,
        ConsultationImageService consultationImageService,
        HairCandidateService hairCandidateService,
        BeardCandidateService beardCandidateService,
        HairPreviewService hairPreviewService)
    {
        _consultationService = consultationService;
        _consultationImageService = consultationImageService;
        _hairCandidateService = hairCandidateService;
        _beardCandidateService = beardCandidateService;
        _hairPreviewService = hairPreviewService;
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
    [HttpPost("/api/consultations/{consultationId:long}/hair-candidates")]
    public async Task<ActionResult<HairCandidateResponse>> AddHairCandidate(
    long consultationId,
    AddHairCandidateRequest request,
    CancellationToken cancellationToken)
    {
        var candidate = await _hairCandidateService.AddHairCandidateAsync(
            consultationId,
            request,
            cancellationToken);

        return Ok(candidate);
    }
    [HttpPost("/api/consultations/{consultationId:long}/beard-candidates")]
    public async Task<ActionResult<BeardCandidateResponse>> AddBeardCandidate(
    long consultationId,
    AddBeardCandidateRequest request,
    CancellationToken cancellationToken)
    {
        var candidate = await _beardCandidateService.AddBeardCandidateAsync(
            consultationId,
            request,
            cancellationToken);

        return Ok(candidate);
    }
    [HttpPut("/api/consultations/{consultationId:long}/hair-candidates/{candidateId:long}/selection")]
    public async Task<ActionResult<HairCandidateResponse>> SelectHairCandidate(
    long consultationId,
    long candidateId,
    CancellationToken cancellationToken)
    {
        var candidate = await _hairCandidateService.SelectHairCandidateAsync(
            consultationId,
            candidateId,
            cancellationToken);

        return Ok(candidate);
    }
    [HttpPut("/api/consultations/{consultationId:long}/beard-candidates/{candidateId:long}/selection")]
    public async Task<ActionResult<BeardCandidateResponse>> SelectBeardCandidate(
    long consultationId,
    long candidateId,
    CancellationToken cancellationToken)
    {
        var candidate = await _beardCandidateService.SelectBeardCandidateAsync(
            consultationId,
            candidateId,
            cancellationToken);

        return Ok(candidate);
    }
    [HttpGet("/api/consultations/{consultationId:long}/hair-candidates")]
    public async Task<ActionResult<List<HairCandidateResponse>>> GetAllHairCandidates(
    long consultationId,
    CancellationToken cancellationToken)
    {
        var candidates = await _hairCandidateService.GetAllAsync(
            consultationId,
            cancellationToken);

        return Ok(candidates);
    }
    [HttpGet("/api/consultations/{consultationId:long}/beard-candidates")]
    public async Task<ActionResult<List<BeardCandidateResponse>>> GetAllBeardCandidates(
    long consultationId,
    CancellationToken cancellationToken)
    {
        var candidates = await _beardCandidateService.GetAllAsync(
            consultationId,
            cancellationToken);

        return Ok(candidates);
    }
    [HttpDelete("/api/consultations/{consultationId:long}/hair-candidates/{candidateId:long}")]
    public async Task<IActionResult> DeleteHairCandidate(
    long consultationId,
    long candidateId,
    CancellationToken cancellationToken)
    {
        await _hairCandidateService.DeleteAsync(
            consultationId,
            candidateId,
            cancellationToken);

        return NoContent();
    }
    [HttpDelete("/api/consultations/{consultationId:long}/beard-candidates/{candidateId:long}")]
    public async Task<IActionResult> DeleteBeardCandidate(
    long consultationId,
    long candidateId,
    CancellationToken cancellationToken)
    {
        await _beardCandidateService.DeleteAsync(
            consultationId,
            candidateId,
            cancellationToken);

        return NoContent();
    }
    [HttpPost("/api/consultations/{consultationId:long}/hair-candidates/{candidateId:long}/preview")]
    public async Task<ActionResult<HairPreviewResponse>> GenerateHairPreview(
    long consultationId,
    long candidateId,
    CancellationToken cancellationToken)
    {
        var result = await _hairPreviewService.GenerateAsync(
            consultationId,
            candidateId,
            cancellationToken);

        return Ok(result);
    }
    [HttpGet("/api/consultations/{consultationId:long}/images/{imageId:long}")]
    public async Task<IActionResult> GetImage(
    long consultationId,
    long imageId,
    CancellationToken cancellationToken)
    {
        var (stream, contentType) =
            await _consultationImageService.GetImageAsync(
                consultationId,
                imageId,
                cancellationToken);

        return File(stream, contentType);
    }
    [HttpGet("/api/consultations/{consultationId:long}/images")]
    public async Task<ActionResult<List<ConsultationImageResponse>>> GetImages(
    long consultationId,
    CancellationToken cancellationToken)
    {
        var images = await _consultationImageService.GetImagesAsync(
            consultationId,
            cancellationToken);

        return Ok(images);
    }
}