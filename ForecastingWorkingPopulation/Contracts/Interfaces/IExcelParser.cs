﻿using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;
using ForecastingWorkingPopulation.Models.Excel;
using OfficeOpenXml;
using System.IO;

namespace ForecastingWorkingPopulation.Contracts.Interfaces
{
    public interface IExcelParser
    {
        /// <summary>
        /// Открывает таблицу по пути
        /// </summary>
        /// <param name="path"></param>
        /// <returns> Все видимые листы </returns>
        public List<ExcelWorksheet> ReadPackage(string path);
        /// <summary>
        /// Возвращает список с типом T полученный из таблицы
        /// </summary>
        /// <typeparam name="TExcelItem"></typeparam>
        /// <returns></returns>
        public List<RegionStatisticsDto> Parse(string path, int startRowNumber, List<int> years, int endColumnNumber = 10);
        public List<RegionStatisticsDto> ParseBiluten(string path, string workSheetName, int columnOffset);
        public List<BirthRateEntity> ParseBirthRateBiluten(string path);
        public void FillForecastFile(string path, string regionName, string forecastName, Dictionary<int, List<RegionStatisticsDto>> forecastDictionary,
            Dictionary<int, List<RegionStatisticsDto>>? retrospectivePermanentData = null, Dictionary<int, List<RegionStatisticsDto>>? retrospectiveEconomyData = null);
        public List<RegionExcelItem> GetBulitenWorksheets(string path, ref BilutenType type);
    }
}
