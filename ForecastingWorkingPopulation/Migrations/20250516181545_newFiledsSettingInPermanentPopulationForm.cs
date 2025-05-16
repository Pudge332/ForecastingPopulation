using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingWorkingPopulation.Migrations
{
    /// <inheritdoc />
    public partial class newFiledsSettingInPermanentPopulationForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeltaValue",
                table: "RegionMainFormSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SelectedCoefficientProcessing",
                table: "RegionMainFormSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeltaValue",
                table: "RegionMainFormSettings");

            migrationBuilder.DropColumn(
                name: "SelectedCoefficientProcessing",
                table: "RegionMainFormSettings");
        }
    }
}
