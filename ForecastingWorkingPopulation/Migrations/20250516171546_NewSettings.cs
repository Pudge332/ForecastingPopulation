using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingWorkingPopulation.Migrations
{
    /// <inheritdoc />
    public partial class NewSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegionForecastionFormSettings",
                columns: table => new
                {
                    RegionNumber = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SelectedGender = table.Column<int>(type: "INTEGER", nullable: false),
                    ForecastStep = table.Column<int>(type: "INTEGER", nullable: false),
                    ForecastEndYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ForecastMaxY = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionForecastionFormSettings", x => x.RegionNumber);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegionForecastionFormSettings");
        }
    }
}
