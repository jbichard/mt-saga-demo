using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTDemo.Sagas.Migrations.SurveyImportStateDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyImports",
                columns: table => new
                {
                    SurveyImportId = table.Column<Guid>(type: "uuid", nullable: false),
                    SurveyData = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyImports", x => x.SurveyImportId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionImport",
                columns: table => new
                {
                    QuestionId = table.Column<string>(type: "text", nullable: false),
                    IsImported = table.Column<bool>(type: "boolean", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: false),
                    SurveyImportId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionImport", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_QuestionImport_SurveyImports_SurveyImportId",
                        column: x => x.SurveyImportId,
                        principalTable: "SurveyImports",
                        principalColumn: "SurveyImportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionImport_SurveyImportId",
                table: "QuestionImport",
                column: "SurveyImportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionImport");

            migrationBuilder.DropTable(
                name: "SurveyImports");
        }
    }
}
