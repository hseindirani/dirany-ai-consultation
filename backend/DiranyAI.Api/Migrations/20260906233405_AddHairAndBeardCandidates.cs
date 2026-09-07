using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiranyAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHairAndBeardCandidates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeardCandidates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConsultationId = table.Column<long>(type: "bigint", nullable: false),
                    BeardStyleId = table.Column<long>(type: "bigint", nullable: false),
                    IsSelected = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeardCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeardCandidates_BeardStyles_BeardStyleId",
                        column: x => x.BeardStyleId,
                        principalTable: "BeardStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeardCandidates_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HairCandidates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConsultationId = table.Column<long>(type: "bigint", nullable: false),
                    HairStyleId = table.Column<long>(type: "bigint", nullable: false),
                    IsSelected = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HairCandidates_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HairCandidates_HairStyles_HairStyleId",
                        column: x => x.HairStyleId,
                        principalTable: "HairStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeardCandidates_BeardStyleId",
                table: "BeardCandidates",
                column: "BeardStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_BeardCandidates_ConsultationId_BeardStyleId",
                table: "BeardCandidates",
                columns: new[] { "ConsultationId", "BeardStyleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HairCandidates_ConsultationId_HairStyleId",
                table: "HairCandidates",
                columns: new[] { "ConsultationId", "HairStyleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HairCandidates_HairStyleId",
                table: "HairCandidates",
                column: "HairStyleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeardCandidates");

            migrationBuilder.DropTable(
                name: "HairCandidates");
        }
    }
}
