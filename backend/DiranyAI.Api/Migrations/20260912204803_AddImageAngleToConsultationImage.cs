using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiranyAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddImageAngleToConsultationImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageAngle",
                table: "ConsultationImages",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Front");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageAngle",
                table: "ConsultationImages");
        }
    }
}
