namespace DiranyAI.Api.AI;

public interface IImageGenerationService
{
    Task<Stream> GenerateHairPreviewAsync(
        Stream originalImage,
        string prompt,
        CancellationToken cancellationToken = default);
}