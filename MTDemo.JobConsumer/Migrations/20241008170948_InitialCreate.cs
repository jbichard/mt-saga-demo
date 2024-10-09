using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTDemo.JobConsumer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobAttemptSaga",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CurrentState = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    JobId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    RetryAttempt = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ServiceAddress = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    InstanceAddress = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Started = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Faulted = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    StatusCheckTokenId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAttemptSaga", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "JobSaga",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CurrentState = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Submitted = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ServiceAddress = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JobTimeout = table.Column<TimeSpan>(type: "INTERVAL DAY(8) TO SECOND(7)", nullable: true),
                    Job = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JobTypeId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    AttemptId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    RetryAttempt = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Started = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Completed = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "INTERVAL DAY(8) TO SECOND(7)", nullable: true),
                    Faulted = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Reason = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JobSlotWaitToken = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    JobRetryDelayToken = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSaga", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "JobTypeSaga",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CurrentState = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ActiveJobCount = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ConcurrentJobLimit = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OverrideJobLimit = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    OverrideLimitExpiration = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ActiveJobs = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Instances = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypeSaga", x => x.CorrelationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobAttemptSaga_JobId_RetryAttempt",
                table: "JobAttemptSaga",
                columns: new[] { "JobId", "RetryAttempt" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobAttemptSaga");

            migrationBuilder.DropTable(
                name: "JobSaga");

            migrationBuilder.DropTable(
                name: "JobTypeSaga");
        }
    }
}
