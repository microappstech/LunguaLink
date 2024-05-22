using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Langua.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoforgroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Groups");
        }
    }
}
