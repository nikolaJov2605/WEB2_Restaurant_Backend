using Microsoft.EntityFrameworkCore.Migrations;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Migrations
{
    public partial class FoodQuantityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Food",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                table: "Food",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "Food");
        }
    }
}
