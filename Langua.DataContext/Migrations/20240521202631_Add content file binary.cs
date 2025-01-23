using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Langua.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Addcontentfilebinary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ContentMessage",
                table: "MessageGroups",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentMessage",
                table: "MessageGroups");
        }
    }
}
