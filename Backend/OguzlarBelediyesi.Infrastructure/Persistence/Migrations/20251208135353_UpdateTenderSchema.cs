using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OguzlarBelediyesi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTenderSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Tenders");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                table: "Tenders",
                newName: "TenderDate");

            migrationBuilder.AddColumn<string>(
                name: "DocumentsJson",
                table: "Tenders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentsJson",
                table: "Tenders");

            migrationBuilder.RenameColumn(
                name: "TenderDate",
                table: "Tenders",
                newName: "PublishedAt");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Tenders",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
