using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiranyAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceStyleCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "a short boxed beard with an even short length, a clean structured shape, defined cheek edges, and a crisp neckline", "A neatly trimmed short beard with a defined boxed shape and clean edges." });

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "a well-groomed fuller beard with natural volume that gradually tapers from the cheeks and jaw toward a longer, clearly defined point at the chin", "A fuller shaped beard that gradually narrows toward a defined point at the chin." });

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "a professionally barbered beard with a zero skin fade starting high on the cheeks and sideburns, gradually and seamlessly blending from almost bare skin into a dense fuller beard along the lower jaw and chin. No sharp or drawn cheek line; the cheek area should naturally fade into thicker facial hair toward the jaw. Keep the moustache completely disconnected from the beard with a clearly visible clean gap at both corners of the mouth. The moustache should be full, dark, neatly shaped and defined. Keep the lower beard full, dense and boxed with a crisp straight lower outline and a slightly fuller chin area. Keep the beard relatively short and sculpted, not long, pointed, rounded or bushy", "A faded boxed beard with near-bare cheeks, increasing density toward the jaw and chin, and a disconnected moustache." });

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "short even designer stubble across the beard area with consistent very short length, natural density, and a clean well-groomed finish", "Very short facial hair maintained at an even length for a clean natural look." });

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "a noticeably longer full beard with dense facial hair covering the cheeks, jawline and chin, substantial natural length and volume below the jaw, a fuller chin area, and a well-groomed balanced shape with clean edges. Keep it full and substantial rather than short or closely trimmed", "A longer, full and dense beard with natural volume across the cheeks, jawline, and chin." });

            migrationBuilder.UpdateData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "a very short buzz cut with an even cropped top, a clean sharp natural lineup around the forehead and temples, and a skin fade on the sides and back blending smoothly into the short top", "A very short buzz cut with a sharp lineup and a clean skin fade on the sides and back." });

            migrationBuilder.InsertData(
                table: "HairStyles",
                columns: new[] { "Id", "AiPromptHint", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 6L, "classic side-part haircut with a clean defined natural side part, neatly combed top with moderate length and volume, and clean tapered shorter sides", "A clean classic haircut with a defined side part, neatly styled top, and tapered sides.", true, "Classic Side Part" },
                    { 7L, "modern quiff with medium-length hair on top, noticeable natural volume at the front styled upward and slightly backward, with shorter clean sides", "A voluminous hairstyle with the front lifted upward and backward while keeping the sides shorter.", true, "Quiff" },
                    { 8L, "modern textured mullet with shorter clean sides, textured medium-length hair on top, and visibly longer layered hair extending at the back", "A modern mullet with shorter sides, textured hair on top, and clearly longer hair at the back.", true, "Mullet" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "short boxed beard with clean defined cheek lines, neckline, and even length", "A neatly trimmed short beard with defined cheek and neckline edges." });

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "well-groomed pointy beard tapering toward a defined point at the chin", "A shaped beard that gradually narrows toward a defined point at the chin." });

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "Italian-style full beard with a structured shape, clean cheek lines, and well-groomed finish", "A full, well-groomed beard with a defined shape and clean contours." });

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "short even designer stubble with clean cheek and neckline edges", "Very short facial hair maintained at an even length for a natural rugged look." });

            migrationBuilder.UpdateData(
                table: "BeardStyles",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "full natural beard with balanced volume, groomed shape, and clean edges", "A full beard with natural volume covering the cheeks, jawline, and chin." });

            migrationBuilder.UpdateData(
                table: "HairStyles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "AiPromptHint", "Description" },
                values: new object[] { "short buzz cut with even length and a clean natural hairline", "A very short, even haircut with a clean and simple finish." });
        }
    }
}
