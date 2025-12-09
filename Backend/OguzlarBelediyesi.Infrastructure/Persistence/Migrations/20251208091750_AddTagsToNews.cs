using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OguzlarBelediyesi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsToNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TagsJson",
                table: "News",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagsJson",
                table: "News");
        }
    }
}
