using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingWorkingPopulation.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PermanentPopulationMaxY",
                table: "RegionMainFormSettings",
                newName: "PermanentPopulationMaximumY");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PermanentPopulationMaximumY",
                table: "RegionMainFormSettings",
                newName: "PermanentPopulationMaxY");
        }
    }
}
