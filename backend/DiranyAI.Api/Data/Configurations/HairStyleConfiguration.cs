using DiranyAI.Api.Styles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiranyAI.Api.Data.Configurations;

public class HairStyleConfiguration : IEntityTypeConfiguration<HairStyle>
{
    public void Configure(EntityTypeBuilder<HairStyle> builder)
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
    new HairStyle
    {
        Id = 1,
        Name = "Taper Fade",
        Description = "A gradual fade around the sideburns and neckline while keeping more length around the sides and top.",
        AiPromptHint = "taper fade haircut with a clean gradual transition around the temples and neckline",
        IsActive = true
    },
    new HairStyle
    {
        Id = 2,
        Name = "Buzz Cut",
        Description = "A very short, even haircut with a clean and simple finish.",
        AiPromptHint = "short buzz cut with even length and a clean natural hairline",
        IsActive = true
    },
    new HairStyle
    {
        Id = 3,
        Name = "Mohawk",
        Description = "Shorter sides with a distinct longer strip of hair through the center.",
        AiPromptHint = "modern mohawk with short sides and a defined longer strip through the center",
        IsActive = true
    },
    new HairStyle
    {
        Id = 4,
        Name = "Textured Crop",
        Description = "Short sides with a textured top styled naturally forward.",
        AiPromptHint = "textured crop haircut with short sides and natural textured hair on top",
        IsActive = true
    },
    new HairStyle
    {
        Id = 5,
        Name = "Slick Back",
        Description = "Longer hair on top styled backward for a clean structured look.",
        AiPromptHint = "slicked-back hairstyle with longer hair on top styled naturally backward",
        IsActive = true
    }
);
    }
}