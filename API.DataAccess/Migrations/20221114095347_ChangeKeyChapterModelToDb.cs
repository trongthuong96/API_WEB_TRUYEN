using Microsoft.EntityFrameworkCore.Migrations;

namespace API.DataAccess.Migrations
{
    public partial class ChangeKeyChapterModelToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters",
                columns: new[] { "Id", "StoryId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters",
                column: "Id");
        }
    }
}
