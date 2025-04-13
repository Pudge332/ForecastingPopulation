using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingWorkingPopulation.Migrations
{
    /// <inheritdoc />
    public partial class EconomyEmploedFormSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegionEconomyEmploedFormSettings",
                columns: table => new
                {
                    RegionNumber = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SelectedGender = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectedSmoothing = table.Column<int>(type: "INTEGER", nullable: false),
                    WindowSize = table.Column<int>(type: "INTEGER", nullable: false),
                    InEconomySelectedSmoothing = table.Column<int>(type: "INTEGER", nullable: false),
                    InEconomyWindowSize = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionEconomyEmploedFormSettings", x => x.RegionNumber);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegionEconomyEmploedFormSettings");
        }
    }
}
