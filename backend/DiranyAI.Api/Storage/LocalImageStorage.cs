namespace DiranyAI.Api.Storage;

public class LocalImageStorage : IImageStorage
{
    private readonly IWebHostEnvironment _environment;

    public LocalImageStorage(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveAsync(
        Stream imageStream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var uploadDirectory = Path.Combine(
            _environment.ContentRootPath,
            "uploads");

        Directory.CreateDirectory(uploadDirectory);

        var uniqueFileName =
            $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        var filePath = Path.Combine(
            uploadDirectory,
            uniqueFileName);

        await using var fileStream = File.Create(filePath);

        await imageStream.CopyToAsync(
            fileStream,
            cancellationToken);

        return $"uploads/{uniqueFileName}";
    }

    public Task<Stream> OpenReadAsync(
        string storagePath,
        CancellationToken cancellationToken = default)
    {
        var normalizedPath = storagePath
            .Replace('\\', Path.DirectorySeparatorChar)
            .Replace('/', Path.DirectorySeparatorChar);

        var fullPath = Path.Combine(
            _environment.ContentRootPath,
            normalizedPath);

        Stream stream = File.OpenRead(fullPath);

        return Task.FromResult(stream);
    }
}