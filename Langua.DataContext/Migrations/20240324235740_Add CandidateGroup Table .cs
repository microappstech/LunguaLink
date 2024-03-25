using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Langua.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddCandidateGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupCandidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    CandidatId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupCandidates_Candidates_CandidatId",
                        column: x => x.CandidatId,
                        principalTable: "Candidates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupCandidates_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupCandidates_CandidatId",
                table: "GroupCandidates",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupCandidates_GroupId",
                table: "GroupCandidates",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupCandidates");
        }
    }
}
