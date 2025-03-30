using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingWorkingPopulation.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployedEconomyPopulations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RegionNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployedEconomyPopulations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermanentPopulations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RegionNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermanentPopulations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionCoefficientSettings",
                columns: table => new
                {
                    RegionNumber = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Coefficient2019 = table.Column<double>(type: "REAL", nullable: false),
                    Coefficient2020 = table.Column<double>(type: "REAL", nullable: false),
                    Coefficient2021 = table.Column<double>(type: "REAL", nullable: false),
                    Coefficient2022 = table.Column<double>(type: "REAL", nullable: false),
                    Coefficient2023 = table.Column<double>(type: "REAL", nullable: false),
                    Coefficient2024 = table.Column<double>(type: "REAL", nullable: false),
                    MinAge = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxAge = table.Column<int>(type: "INTEGER", nullable: false),
                    CoefficientLimit = table.Column<decimal>(type: "TEXT", nullable: false),
                    DisableCoefficientCutoff = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionCoefficientSettings", x => x.RegionNumber);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Number = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Number);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployedEconomyPopulations");

            migrationBuilder.DropTable(
                name: "PermanentPopulations");

            migrationBuilder.DropTable(
                name: "RegionCoefficientSettings");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
