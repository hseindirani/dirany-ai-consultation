using DiranyAI.Api.Consultations;
using DiranyAI.Api.Customers;

using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<ConsultationImage> ConsultationImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}