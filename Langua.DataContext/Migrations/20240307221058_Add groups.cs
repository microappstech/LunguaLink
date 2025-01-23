using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Langua.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Addgroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupsId",
                table: "MessageGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupsId",
                table: "Candidates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageGroups_GroupsId",
                table: "MessageGroups",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_GroupsId",
                table: "Candidates",
                column: "GroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Groups_GroupsId",
                table: "Candidates",
                column: "GroupsId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageGroups_Groups_GroupsId",
                table: "MessageGroups",
                column: "GroupsId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Groups_GroupsId",
                table: "Candidates");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageGroups_Groups_GroupsId",
                table: "MessageGroups");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_MessageGroups_GroupsId",
                table: "MessageGroups");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_GroupsId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "GroupsId",
                table: "MessageGroups");

            migrationBuilder.DropColumn(
                name: "GroupsId",
                table: "Candidates");
        }
    }
}
