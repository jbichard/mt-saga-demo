using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTDemo.Sagas.Migrations.SurveyImportSagaDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyImportSagaState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CurrentState = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    ImportStartDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SurveyImportDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SurveyPublishDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ImportEndDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Success = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "RAW(8)", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyImportSagaState", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyImportSagaState");
        }
    }
}
