using Microsoft.EntityFrameworkCore.Migrations;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Migrations
{
    public partial class verificationDeniedFieldMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Denied",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Denied",
                table: "Users");
        }
    }
}
