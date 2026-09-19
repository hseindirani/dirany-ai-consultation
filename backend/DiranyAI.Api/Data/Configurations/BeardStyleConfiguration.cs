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
         Description = "A neatly trimmed short beard with a defined boxed shape and clean edges.",
         AiPromptHint = "a short boxed beard with an even short length, a clean structured shape, defined cheek edges, and a crisp neckline",
         IsActive = true
     },
     new BeardStyle
     {
         Id = 2,
         Name = "Pointy Beard",
         Description = "A fuller shaped beard that gradually narrows toward a defined point at the chin.",
         AiPromptHint = "a well-groomed fuller beard with natural volume that gradually tapers from the cheeks and jaw toward a longer, clearly defined point at the chin",
         IsActive = true
     },
     new BeardStyle
     {
         Id = 3,
         Name = "Italian Beard",
         Description = "A faded boxed beard with near-bare cheeks, increasing density toward the jaw and chin, and a disconnected moustache.",
         AiPromptHint = "a professionally barbered beard with a zero skin fade starting high on the cheeks and sideburns, gradually and seamlessly blending from almost bare skin into a dense fuller beard along the lower jaw and chin. No sharp or drawn cheek line; the cheek area should naturally fade into thicker facial hair toward the jaw. Keep the moustache completely disconnected from the beard with a clearly visible clean gap at both corners of the mouth. The moustache should be full, dark, neatly shaped and defined. Keep the lower beard full, dense and boxed with a crisp straight lower outline and a slightly fuller chin area. Keep the beard relatively short and sculpted, not long, pointed, rounded or bushy",
         IsActive = true
     },
     new BeardStyle
     {
         Id = 4,
         Name = "Stubble",
         Description = "Very short facial hair maintained at an even length for a clean natural look.",
         AiPromptHint = "short even designer stubble across the beard area with consistent very short length, natural density, and a clean well-groomed finish",
         IsActive = true
     },
     new BeardStyle
     {
         Id = 5,
         Name = "Full Beard",
         Description = "A longer, full and dense beard with natural volume across the cheeks, jawline, and chin.",
         AiPromptHint = "a noticeably longer full beard with dense facial hair covering the cheeks, jawline and chin, substantial natural length and volume below the jaw, a fuller chin area, and a well-groomed balanced shape with clean edges. Keep it full and substantial rather than short or closely trimmed",
         IsActive = true
     }
 );
    }
    }