namespace DiranyAI.Api.Storage;

public interface IImageStorage
{
    Task<string> SaveAsync(
        Stream imageStream,
        string fileName,
        CancellationToken cancellationToken = default);
}