namespace DiranyAI.Api.AI;

public class ImageInput
{
    public required Stream Stream { get; init; }

    public required string FileName { get; init; }

    public required string ContentType { get; init; }
}