using Microsoft.EntityFrameworkCore.Migrations;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Migrations
{
    public partial class pictureUploadMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFilePath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFilePath",
                table: "Users");
        }
    }
}
