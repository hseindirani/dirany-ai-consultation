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
        Description = "A very short buzz cut with a sharp lineup and a clean skin fade on the sides and back.",
        AiPromptHint = "a very short buzz cut with an even cropped top, a clean sharp natural lineup around the forehead and temples, and a skin fade on the sides and back blending smoothly into the short top",
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
    },
    new HairStyle
    {
        Id = 6,
        Name = "Classic Side Part",
        Description = "A clean classic haircut with a defined side part, neatly styled top, and tapered sides.",
        AiPromptHint = "classic side-part haircut with a clean defined natural side part, neatly combed top with moderate length and volume, and clean tapered shorter sides",
        IsActive = true
    },
    new HairStyle
    {
        Id = 7,
        Name = "Quiff",
        Description = "A voluminous hairstyle with the front lifted upward and backward while keeping the sides shorter.",
        AiPromptHint = "modern quiff with medium-length hair on top, noticeable natural volume at the front styled upward and slightly backward, with shorter clean sides",
        IsActive = true
    },
    new HairStyle
    {
        Id = 8,
        Name = "Mullet",
        Description = "A modern mullet with shorter sides, textured hair on top, and clearly longer hair at the back.",
        AiPromptHint = "modern textured mullet with shorter clean sides, textured medium-length hair on top, and visibly longer layered hair extending at the back",
        IsActive = true
    }
);
    }
}