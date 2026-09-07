using DiranyAI.Api.Consultations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiranyAI.Api.Data.Configurations;

public class HairCandidateConfiguration
    : IEntityTypeConfiguration<HairCandidate>
{
    public void Configure(EntityTypeBuilder<HairCandidate> builder)
    {
        builder.HasOne(c => c.Consultation)
            .WithMany(c => c.HairCandidates)
            .HasForeignKey(c => c.ConsultationId)
            .IsRequired();

        builder.HasOne(c => c.HairStyle)
            .WithMany(s => s.HairCandidates)
            .HasForeignKey(c => c.HairStyleId)
            .IsRequired();

        builder.HasIndex(c => new
        {
            c.ConsultationId,
            c.HairStyleId
        })
        .IsUnique();
    }
}