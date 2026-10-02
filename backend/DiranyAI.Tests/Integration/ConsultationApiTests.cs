using System.Net;
using DiranyAI.Api.Consultations;
using DiranyAI.Api.Customers;
using DiranyAI.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace DiranyAI.Tests.Integration;

public class ConsultationApiTests
{
    [Fact]
    public async Task CompleteConsultation_WithoutFinalResult_ReturnsBadRequest()
    {
        await using var factory = new CustomWebApplicationFactory();
        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            await context.Database.EnsureCreatedAsync();
        }

        long consultationId;

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            var customer = new Customer
            {
                FirstName = "Test",
                LastName = "Customer",
                PhoneNumber = "12345678",
                CreatedAt = DateTimeOffset.UtcNow
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var consultation = new Consultation
            {
                CustomerId = customer.Id,
                Status = ConsultationStatus.InProgress,
                CreatedAt = DateTimeOffset.UtcNow
            };

            context.Consultations.Add(consultation);
            await context.SaveChangesAsync();

            consultationId = consultation.Id;
        }

        var client = factory.CreateClient();
        var csrfResponse = await client.GetAsync("/api/auth/csrf-token");
        csrfResponse.EnsureSuccessStatusCode();

        var csrfData = await csrfResponse.Content
            .ReadFromJsonAsync<CsrfTokenResponse>();

        client.DefaultRequestHeaders.Add(
            "X-CSRF-TOKEN",
            csrfData!.Token);

        var response = await client.PostAsync(
            $"/api/consultations/{consultationId}/complete",
            null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    private sealed class CsrfTokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
    [Fact]
    public async Task ProtectedEndpoint_WithoutAuthentication_ReturnsUnauthorized()
    {
        await using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        client.DefaultRequestHeaders.Add(
            "X-Test-Anonymous",
            "true");

        var response = await client.GetAsync("/api/hair-styles");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
    [Fact]
    public async Task UnsafeRequest_WithoutCsrfToken_ReturnsBadRequest()
    {
        await using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/customers",
            new
            {
                firstName = "Csrf",
                lastName = "Test",
                phoneNumber = "12345678"
            });

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}
