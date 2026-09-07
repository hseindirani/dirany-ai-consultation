using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiranyAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHairCandidateToConsultationImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HairCandidateId",
                table: "ConsultationImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationImages_HairCandidateId",
                table: "ConsultationImages",
                column: "HairCandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultationImages_HairCandidates_HairCandidateId",
                table: "ConsultationImages",
                column: "HairCandidateId",
                principalTable: "HairCandidates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultationImages_HairCandidates_HairCandidateId",
                table: "ConsultationImages");

            migrationBuilder.DropIndex(
                name: "IX_ConsultationImages_HairCandidateId",
                table: "ConsultationImages");

            migrationBuilder.DropColumn(
                name: "HairCandidateId",
                table: "ConsultationImages");
        }
    }
}
