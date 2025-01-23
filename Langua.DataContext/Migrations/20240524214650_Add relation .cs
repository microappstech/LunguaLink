using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Langua.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Addrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "MessageGroups",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_MessageGroups_SenderId",
                table: "MessageGroups",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageGroups_Users_SenderId",
                table: "MessageGroups",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageGroups_Users_SenderId",
                table: "MessageGroups");

            migrationBuilder.DropIndex(
                name: "IX_MessageGroups_SenderId",
                table: "MessageGroups");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "MessageGroups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
