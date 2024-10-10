using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTDemo.Sagas.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyImportState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentState = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    SurveyImportId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SurveyImportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SurveyPublishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GetImportDetailsRequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyImportState", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "ConditionImportState",
                columns: table => new
                {
                    ConditionId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsImported = table.Column<bool>(type: "boolean", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionImportState", x => x.ConditionId);
                    table.ForeignKey(
                        name: "FK_ConditionImportState_SurveyImportState_CorrelationId",
                        column: x => x.CorrelationId,
                        principalTable: "SurveyImportState",
                        principalColumn: "CorrelationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionImportState",
                columns: table => new
                {
                    QuestionId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsImported = table.Column<bool>(type: "boolean", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionImportState", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_QuestionImportState_SurveyImportState_CorrelationId",
                        column: x => x.CorrelationId,
                        principalTable: "SurveyImportState",
                        principalColumn: "CorrelationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConditionImportState_CorrelationId",
                table: "ConditionImportState",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionImportState_CorrelationId",
                table: "QuestionImportState",
                column: "CorrelationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConditionImportState");

            migrationBuilder.DropTable(
                name: "QuestionImportState");

            migrationBuilder.DropTable(
                name: "SurveyImportState");
        }
    }
}
