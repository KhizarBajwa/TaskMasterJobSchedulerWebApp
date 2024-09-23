using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionLogs",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunInstanceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduleFireTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    FireTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    JobRunTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVetoed = table.Column<bool>(type: "bit", nullable: false),
                    IsException = table.Column<bool>(type: "bit", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ReturnCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAddedUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionLogs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "JobSchedules",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfThreads = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CronTriggerExpression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSchedules", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "JobRunLogs",
                columns: table => new
                {
                    JobRunId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobSchedule_JobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRunLogs", x => x.JobRunId);
                    table.ForeignKey(
                        name: "FK_JobRunLogs_JobSchedules_JobSchedule_JobId",
                        column: x => x.JobSchedule_JobId,
                        principalTable: "JobSchedules",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobRunLogs_JobSchedule_JobId",
                table: "JobRunLogs",
                column: "JobSchedule_JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionLogs");

            migrationBuilder.DropTable(
                name: "JobRunLogs");

            migrationBuilder.DropTable(
                name: "JobSchedules");
        }
    }
}
