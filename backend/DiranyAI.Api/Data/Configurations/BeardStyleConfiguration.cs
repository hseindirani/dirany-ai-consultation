using DiranyAI.Api.Styles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiranyAI.Api.Data.Configurations;

public class BeardStyleConfiguration : IEntityTypeConfiguration<BeardStyle>
{
    public void Configure(EntityTypeBuilder<BeardStyle> builder)
    {
        builder.Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(s => s.Name)
            .IsUnique();

        builder.Property(s => s.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.AiPromptHint)
            .HasMaxLength(1000)
            .IsRequired();
        builder.HasData(
    new BeardStyle
    {
        Id = 1,
        Name = "Short Boxed Beard",
        Description = "A neatly trimmed short beard with defined cheek and neckline edges.",
        AiPromptHint = "short boxed beard with clean defined cheek lines, neckline, and even length",
        IsActive = true
    },
    new BeardStyle
    {
        Id = 2,
        Name = "Pointy Beard",
        Description = "A shaped beard that gradually narrows toward a defined point at the chin.",
        AiPromptHint = "well-groomed pointy beard tapering toward a defined point at the chin",
        IsActive = true
    },
    new BeardStyle
    {
        Id = 3,
        Name = "Italian Beard",
        Description = "A full, well-groomed beard with a defined shape and clean contours.",
        AiPromptHint = "Italian-style full beard with a structured shape, clean cheek lines, and well-groomed finish",
        IsActive = true
    },
    new BeardStyle
    {
        Id = 4,
        Name = "Stubble",
        Description = "Very short facial hair maintained at an even length for a natural rugged look.",
        AiPromptHint = "short even designer stubble with clean cheek and neckline edges",
        IsActive = true
    },
    new BeardStyle
    {
        Id = 5,
        Name = "Full Beard",
        Description = "A full beard with natural volume covering the cheeks, jawline, and chin.",
        AiPromptHint = "full natural beard with balanced volume, groomed shape, and clean edges",
        IsActive = true
    }
);
    }
}