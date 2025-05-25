using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogAnalizerServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlarmLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeWhenLogged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocalZoneTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false),
                    AlarmId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AlarmClass = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Resource = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LoggedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PrevState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LogAction = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FinalState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AlarmMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GenerationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GenerationTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Project = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WeekType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComparisonResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlarmMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CountWeek1 = table.Column<int>(type: "int", nullable: false),
                    CountWeek2 = table.Column<int>(type: "int", nullable: false),
                    Week1Type = table.Column<int>(type: "int", nullable: false),
                    Week2Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlarmLogs");

            migrationBuilder.DropTable(
                name: "ComparisonResults");
        }
    }
}
