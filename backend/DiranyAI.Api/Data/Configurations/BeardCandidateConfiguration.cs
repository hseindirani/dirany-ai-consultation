using DiranyAI.Api.Consultations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiranyAI.Api.Data.Configurations;

public class BeardCandidateConfiguration
    : IEntityTypeConfiguration<BeardCandidate>
{
    public void Configure(EntityTypeBuilder<BeardCandidate> builder)
    {
        builder.HasOne(c => c.Consultation)
            .WithMany(c => c.BeardCandidates)
            .HasForeignKey(c => c.ConsultationId)
            .IsRequired();

        builder.HasOne(c => c.BeardStyle)
            .WithMany(s => s.BeardCandidates)
            .HasForeignKey(c => c.BeardStyleId)
            .IsRequired();

        builder.HasIndex(c => new
        {
            c.ConsultationId,
            c.BeardStyleId
        })
        .IsUnique();
    }
}