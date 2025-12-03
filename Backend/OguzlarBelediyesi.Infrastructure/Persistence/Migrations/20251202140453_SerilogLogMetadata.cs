using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OguzlarBelediyesi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SerilogLogMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DurationMs",
                table: "SerilogLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "SerilogLogs",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "SerilogLogs",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMs",
                table: "SerilogLogs");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "SerilogLogs");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "SerilogLogs");
        }
    }
}
