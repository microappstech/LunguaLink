using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Langua.DataContext.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05d7087c-6ad5-4b9a-b77b-9e647386c5fd", null, "TEACHER", "Teacher" },
                    { "90564021-90e1-4b69-996f-515512d124bf", null, "MANAGER", "Manager" },
                    { "ccdc6a90-dd12-4481-b365-5996bfae957a", null, "ADMIN", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "05d7087c-6ad5-4b9a-b77b-9e647386c5fd");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "90564021-90e1-4b69-996f-515512d124bf");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "ccdc6a90-dd12-4481-b365-5996bfae957a");
        }
    }
}
