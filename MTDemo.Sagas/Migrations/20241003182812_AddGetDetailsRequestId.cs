using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTDemo.Sagas.Migrations
{
    /// <inheritdoc />
    public partial class AddGetDetailsRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GetImportDetailsRequestId",
                table: "SurveyImportState",
                type: "RAW(16)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GetImportDetailsRequestId",
                table: "SurveyImportState");
        }
    }
}
