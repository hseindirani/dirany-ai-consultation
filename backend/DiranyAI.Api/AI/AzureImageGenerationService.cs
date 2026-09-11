using Microsoft.Extensions.Options;

namespace DiranyAI.Api.AI;

public class AzureImageGenerationService : IImageGenerationService
{
    private readonly HttpClient _httpClient;
    private readonly AzureOpenAIOptions _options;

    public AzureImageGenerationService(
        HttpClient httpClient,
        IOptions<AzureOpenAIOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<Stream> GenerateHairPreviewAsync(
        ImageInput originalImage,
        string prompt,
        CancellationToken cancellationToken = default)
    {
        using var form = new MultipartFormDataContent();

        form.Add(
            new StringContent(_options.DeploymentName),
            "model");

        form.Add(
            new StringContent(prompt),
            "prompt");

        var imageContent = new StreamContent(originalImage.Stream);
        imageContent.Headers.ContentType =
              new System.Net.Http.Headers.MediaTypeHeaderValue(
                      originalImage.ContentType);

        form.Add(
            imageContent,
            "image",
            originalImage.FileName);

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{_options.Endpoint}/images/edits?api-version=preview");

        request.Headers.Add("api-key", _options.ApiKey);
        request.Content = form;

        using var response = await _httpClient.SendAsync(
                        request,
                       cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new InvalidOperationException(
                $"Azure OpenAI image edit failed with status {(int)response.StatusCode}: {errorBody}");
        }

        var responseBody = await response.Content.ReadFromJsonAsync<ImageEditResponse>(
           cancellationToken);

        var base64Image = responseBody?.Data.FirstOrDefault()?.Base64Json;

        if (string.IsNullOrWhiteSpace(base64Image))
        {
            throw new InvalidOperationException(
                "Azure OpenAI did not return a generated image.");
        }

        var imageBytes = Convert.FromBase64String(base64Image);

        return new MemoryStream(imageBytes);
    }
}
internal class ImageEditResponse
{
    public List<ImageEditData> Data { get; set; } = [];
}

internal class ImageEditData
{
    [System.Text.Json.Serialization.JsonPropertyName("b64_json")]
    public string? Base64Json { get; set; }
}