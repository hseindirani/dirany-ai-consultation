namespace DiranyAI.Api.AI;

public interface IImageGenerationService
{
    Task<Stream> GenerateHairPreviewAsync(
        ImageInput originalImage,
        string prompt,
        CancellationToken cancellationToken = default);
}