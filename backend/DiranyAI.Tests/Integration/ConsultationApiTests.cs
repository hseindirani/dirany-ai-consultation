using System.Net;
using DiranyAI.Api.Consultations;
using DiranyAI.Api.Customers;
using DiranyAI.Api.Data;
using Microsoft.Extensions.DependencyInjection;

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

        var response = await client.PostAsync(
            $"/api/consultations/{consultationId}/complete",
            null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}