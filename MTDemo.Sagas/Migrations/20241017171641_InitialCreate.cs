using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTDemo.Sagas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyImports",
                columns: table => new
                {
                    SurveyImportId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    SurveyData = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyImports", x => x.SurveyImportId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionImport",
                columns: table => new
                {
                    QuestionId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    SurveyImportId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Sequence = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsImported = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    Error = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionImport", x => new { x.SurveyImportId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_QuestionImport_SurveyImports_SurveyImportId",
                        column: x => x.SurveyImportId,
                        principalTable: "SurveyImports",
                        principalColumn: "SurveyImportId",
                        onDelete: ReferentialAction.Cascade);
                });
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
