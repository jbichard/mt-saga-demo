using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTDemo.Sagas.Migrations.SurveyImportStateDb
{
    /// <inheritdoc />
    public partial class QuestionPKFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionImport",
                table: "QuestionImport");

            migrationBuilder.DropIndex(
                name: "IX_QuestionImport_SurveyImportId",
                table: "QuestionImport");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionImport",
                table: "QuestionImport",
                columns: new[] { "SurveyImportId", "QuestionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionImport",
                table: "QuestionImport");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionImport",
                table: "QuestionImport",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionImport_SurveyImportId",
                table: "QuestionImport",
                column: "SurveyImportId");
        }
    }
}
