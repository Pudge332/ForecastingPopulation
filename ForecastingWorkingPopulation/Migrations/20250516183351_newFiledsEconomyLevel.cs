using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingWorkingPopulation.Migrations
{
    /// <inheritdoc />
    public partial class newFiledsEconomyLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InEconomyLevelMaxAge",
                table: "RegionEconomyEmploedFormSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InEconomyLevelMinAge",
                table: "RegionEconomyEmploedFormSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InEconomyLevelMaxAge",
                table: "RegionEconomyEmploedFormSettings");

            migrationBuilder.DropColumn(
                name: "InEconomyLevelMinAge",
                table: "RegionEconomyEmploedFormSettings");
        }
    }
}
