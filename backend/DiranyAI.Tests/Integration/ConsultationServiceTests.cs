using DiranyAI.Api.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using DiranyAI.Api.Consultations;
using DiranyAI.Api.Customers;
using Microsoft.Extensions.Logging.Abstractions;

namespace DiranyAI.Tests.Integration;

public class ConsultationServiceTests
{
    private static async Task<(AppDbContext Context, SqliteConnection Connection)>
        CreateDbContextAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        return (context, connection);
    }
    [Fact]
    public async Task CompleteAsync_WithoutFinalResult_ThrowsArgumentException()
    {
        var (context, connection) = await CreateDbContextAsync();

        await using (context)
        await using (connection)
        {
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

            var service = new ConsultationService(
                context,
                NullLogger<ConsultationService>.Instance);


            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => service.CompleteAsync(consultation.Id));

            Assert.Equal(
                "A final result image must be uploaded before completing the consultation.",
                exception.Message);
        }
    }
    [Fact]
    public async Task CompleteAsync_WithFinalResult_CompletesConsultation()
    {
        var (context, connection) = await CreateDbContextAsync();

        await using (context)
        await using (connection)
        {
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

            var finalResult = new ConsultationImage
            {
                ConsultationId = consultation.Id,
                ImageType = ConsultationImageType.FinalResult,
                ImageAngle = ConsultationImageAngle.Front,
                StoragePath = "test-final-result.jpg",
                CreatedAt = DateTimeOffset.UtcNow
            };

            context.ConsultationImages.Add(finalResult);
            await context.SaveChangesAsync();

            var service = new ConsultationService(
                          context,
                          NullLogger<ConsultationService>.Instance);

            var result = await service.CompleteAsync(consultation.Id);

            Assert.Equal(ConsultationStatus.Completed, result.Status);
            Assert.NotNull(result.CompletedAt);
        }
    }
}