using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTDemo.Sagas.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveyImportId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignedUri",
                table: "SurveyImportState");

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyImportId",
                table: "SurveyImportState",
                type: "RAW(16)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SurveyImportId",
                table: "SurveyImportState");

            migrationBuilder.AddColumn<string>(
                name: "SignedUri",
                table: "SurveyImportState",
                type: "NVARCHAR2(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
