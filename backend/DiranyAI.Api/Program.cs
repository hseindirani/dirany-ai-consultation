using DiranyAI.Api.Common.Middleware;
using DiranyAI.Api.Consultations;
using DiranyAI.Api.Customers;
using DiranyAI.Api.Data;
using DiranyAI.Api.Storage;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ConsultationService>();
builder.Services.AddScoped<ConsultationImageService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IImageStorage, LocalImageStorage>();

var app = builder.Build();
app.UseExceptionHandler();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();