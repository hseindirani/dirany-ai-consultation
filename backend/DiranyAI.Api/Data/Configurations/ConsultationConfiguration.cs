using DiranyAI.Api.Consultations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiranyAI.Api.Data.Configurations;

public class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
{
    public void Configure(EntityTypeBuilder<Consultation> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Notes)
            .HasMaxLength(1000);

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasOne(c => c.Customer)
            .WithMany(c => c.Consultations)
            .HasForeignKey(c => c.CustomerId)
            .IsRequired();
    }
}