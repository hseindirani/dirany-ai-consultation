using Azure.Storage.Blobs;

namespace DiranyAI.Api.Storage;

public class AzureBlobImageStorage : IImageStorage
{
    private readonly BlobContainerClient _containerClient;
    public AzureBlobImageStorage(
    BlobServiceClient blobServiceClient,
    IConfiguration configuration)
    {
        var containerName =
            configuration["AzureStorage:ContainerName"]
            ?? throw new InvalidOperationException(
                "Azure Storage container name is not configured.");

        _containerClient =
            blobServiceClient.GetBlobContainerClient(containerName);
    }
    public async Task<string> SaveAsync(
    Stream imageStream,
    string fileName,
    CancellationToken cancellationToken = default)
    {
        var blobName =
            $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        var blobClient =
            _containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(
            imageStream,
            overwrite: false,
            cancellationToken);

        return blobName;
    }

    public async Task<Stream> OpenReadAsync(
    string storagePath,
    CancellationToken cancellationToken = default)
    {
        var blobClient =
            _containerClient.GetBlobClient(storagePath);

        var response =
            await blobClient.DownloadStreamingAsync(
                cancellationToken: cancellationToken);

        return response.Value.Content;
    }
}