using DiranyAI.Api.Consultations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiranyAI.Api.Data.Configurations;

public class ConsultationImageConfiguration
    : IEntityTypeConfiguration<ConsultationImage>
{
    public void Configure(EntityTypeBuilder<ConsultationImage> builder)
    {
        builder.Property(i => i.ImageType)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(i => i.StoragePath)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(i => i.Consultation)
            .WithMany(c => c.Images)
            .HasForeignKey(i => i.ConsultationId)
            .IsRequired();
        builder.HasOne(i => i.HairCandidate)
            .WithMany()
            .HasForeignKey(i => i.HairCandidateId);
        builder.HasOne(i => i.BeardCandidate)
            .WithMany()
            .HasForeignKey(i => i.BeardCandidateId);
    }
}