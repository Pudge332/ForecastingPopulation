using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.Excel;
using ForecastingWorkingPopulation.Models.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace ForecastingWorkingPopulation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            SetDependecies();
            Application.Run(new MainForm(new PopulationRepository()));
        }
        //static void Main(string[] args) 
        //{
        //    var repo = new PopulationRepository();
        //    var inPero = repo.GetEconomyEmployedInRegion(10);
        //    string path = "C:\\Excels\\Республика Карелия.xlsx";
        //    IExcelParser parser = new ExcelParser();
        //    var worksheets = parser.ReadPackage(path);
        //    var allRows = new List<RegionStatisticsDto>();
        //    foreach (var worksheet in worksheets)
        //    {
        //        var result = parser.Parse(worksheet, 5, new List<int> { 2019, 2020, 2021, 2022, 2023 });
        //        allRows.AddRange(result);
        //    }

        //    //allRows.Sort();
        //    repo.SaveEntityByRegion<EmployedEconomyPopulationInRegionEntity>(10, allRows);
            
        //}

        private static void SetDependecies()
        {
            var services = new ServiceCollection();
            services.AddScoped<IExcelParser, ExcelParser>();
            services.AddScoped<IPopulationRepository, PopulationRepository>();

            var serviceProvider = services.BuildServiceProvider();
        }
    }
}