using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Langua.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class langua2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Users_UserId",
                table: "Candidates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_ApplicationUser_UserId",
                table: "Candidates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
