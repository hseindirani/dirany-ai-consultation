using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiranyAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedHairAndBeardStyles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BeardStyles",
                columns: new[] { "Id", "AiPromptHint", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1L, "short boxed beard with clean defined cheek lines, neckline, and even length", "A neatly trimmed short beard with defined cheek and neckline edges.", true, "Short Boxed Beard" },
                    { 2L, "well-groomed pointy beard tapering toward a defined point at the chin", "A shaped beard that gradually narrows toward a defined point at the chin.", true, "Pointy Beard" },
                    { 3L, "Italian-style full beard with a structured shape, clean cheek lines, and well-groomed finish", "A full, well-groomed beard with a defined shape and clean contours.", true, "Italian Beard" },
                    { 4L, "short even designer stubble with clean cheek and neckline edges", "Very short facial hair maintained at an even length for a natural rugged look.", true, "Stubble" },
                    { 5L, "full natural beard with balanced volume, groomed shape, and clean edges", "A full beard with natural volume covering the cheeks, jawline, and chin.", true, "Full Beard" }
                });

            migrationBuilder.InsertData(
                table: "HairStyles",
                columns: new[] { "Id", "AiPromptHint", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1L, "taper fade haircut with a clean gradual transition around the temples and neckline", "A gradual fade around the sideburns and neckline while keeping more length around the sides and top.", true, "Taper Fade" },
                    { 2L, "short buzz cut with even length and a clean natural hairline", "A very short, even haircut with a clean and simple finish.", true, "Buzz Cut" },
                    { 3L, "modern mohawk with short sides and a defined longer strip through the center", "Shorter sides with a distinct longer strip of hair through the center.", true, "Mohawk" },
                    { 4L, "textured crop haircut with short sides and natural textured hair on top", "Short sides with a textured top styled naturally forward.", true, "Textured Crop" },
                    { 5L, "slicked-back hairstyle with longer hair on top styled naturally backward", "Longer hair on top styled backward for a clean structured look.", true, "Slick Back" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 5L);
        }
    }
}
