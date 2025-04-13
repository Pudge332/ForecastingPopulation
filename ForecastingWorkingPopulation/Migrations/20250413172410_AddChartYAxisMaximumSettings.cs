using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingWorkingPopulation.Migrations
{
    /// <inheritdoc />
    public partial class AddChartYAxisMaximumSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EconomyEmploedMaxY",
                table: "RegionEconomyEmploedFormSettings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EconomyEmploedSmoothMaxY",
                table: "RegionEconomyEmploedFormSettings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InEconomyLevelMaxY",
                table: "RegionEconomyEmploedFormSettings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InEconomyLevelSmoothMaxY",
                table: "RegionEconomyEmploedFormSettings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EconomyEmploedMaxY",
                table: "RegionEconomyEmploedFormSettings");

            migrationBuilder.DropColumn(
                name: "EconomyEmploedSmoothMaxY",
                table: "RegionEconomyEmploedFormSettings");

            migrationBuilder.DropColumn(
                name: "InEconomyLevelMaxY",
                table: "RegionEconomyEmploedFormSettings");

            migrationBuilder.DropColumn(
                name: "InEconomyLevelSmoothMaxY",
                table: "RegionEconomyEmploedFormSettings");
        }
    }
}
