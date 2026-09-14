using DiranyAI.Api.AI;
using DiranyAI.Api.Common.Middleware;
using DiranyAI.Api.Consultations;
using DiranyAI.Api.Customers;
using DiranyAI.Api.Data;
using DiranyAI.Api.Storage;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Azure.Storage.Blobs;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<AzureOpenAIOptions>(
    builder.Configuration.GetSection(AzureOpenAIOptions.SectionName));
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ConsultationService>();
builder.Services.AddScoped<ConsultationImageService>();
builder.Services.AddScoped<HairCandidateService>();
builder.Services.AddScoped<BeardCandidateService>();
builder.Services.AddHttpClient<
    IImageGenerationService,
    AzureImageGenerationService>();
builder.Services.AddScoped<HairPreviewService>();
builder.Services.AddScoped<BeardPreviewService>();
builder.Services.AddScoped<CombinedPreviewService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var storageProvider =
    builder.Configuration["Storage:Provider"] ?? "Local";

if (storageProvider.Equals(
    "Azure",
    StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IImageStorage, AzureBlobImageStorage>();
}
else
{
    builder.Services.AddScoped<IImageStorage, LocalImageStorage>();
}
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddSingleton<BlobServiceClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var accountName =
        configuration["AzureStorage:AccountName"]
        ?? throw new InvalidOperationException(
            "Azure Storage account name is not configured.");

    return new BlobServiceClient(
        new Uri($"https://{accountName}.blob.core.windows.net"),
        new DefaultAzureCredential());
});

var app = builder.Build();
app.UseExceptionHandler();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");

app.MapControllers();

app.Run();