using Microsoft.EntityFrameworkCore.Migrations;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Migrations
{
    public partial class IngredientsPriceMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Ingredients");
        }
    }
}
