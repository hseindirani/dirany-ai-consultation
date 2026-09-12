using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiranyAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBeardCandidateToConsultationImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BeardCandidateId",
                table: "ConsultationImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationImages_BeardCandidateId",
                table: "ConsultationImages",
                column: "BeardCandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultationImages_BeardCandidates_BeardCandidateId",
                table: "ConsultationImages",
                column: "BeardCandidateId",
                principalTable: "BeardCandidates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultationImages_BeardCandidates_BeardCandidateId",
                table: "ConsultationImages");

            migrationBuilder.DropIndex(
                name: "IX_ConsultationImages_BeardCandidateId",
                table: "ConsultationImages");

            migrationBuilder.DropColumn(
                name: "BeardCandidateId",
                table: "ConsultationImages");
        }
    }
}
