using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Models.Attributes;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;
using ForecastingWorkingPopulation.Models.Excel;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Reflection;
using System.Windows.Forms;
using static OfficeOpenXml.ExcelErrorValue;

namespace ForecastingWorkingPopulation.Infrastructure.Excel
{
    public class ExcelParser : IExcelParser
    {
        private List<RegionInfoEntity> Regions = RegionRepository.GetRegions();
        public List<RegionStatisticsDto> Parse(string path, int startRowNumber, List<int> years, int endColumnNumber = 10)
        {
            var preResult = new List<RegionStatisticsExcelItem>();
            var result = new List<RegionStatisticsDto>();

            using (var package = new ExcelPackage(path))
            {
                var worksheet = package.Workbook.Worksheets[0];

                foreach (var year in years)
                {
                    for (int rowNumber = startRowNumber; !string.IsNullOrWhiteSpace(worksheet.Cells[rowNumber, 1].Text); rowNumber++)
                    {
                        var item = new RegionStatisticsExcelItem();
                        item.TargetYear = year;
                        foreach (var property in typeof(RegionStatisticsExcelItem).GetProperties())
                        {
                            var attribute = property.GetCustomAttribute<ExcelAttribute>();

                            if (attribute != null)
                            {
                                if(attribute.Year)
                                    property.SetValue(item, worksheet.Cells[rowNumber, attribute.ColumnNumber + year - 2019].Value);
                                else
                                    property.SetValue(item, worksheet.Cells[rowNumber, attribute.ColumnNumber].Value);
                            }
                        }

                        preResult.Add(item);
                    }
                }
            }

            var males = preResult.Where(x => x.Gender == "Мужчины");
            var females = preResult.Where(x => x.Gender != "Мужчины");

            CalculateByAge(result, males);
            CalculateByAge(result, females);

            return result;
        }

        public List<RegionStatisticsDto> ParseBiluten(string path, string workSheetName, int columnOffset)
        {
            var defaultColumnNumber = 1;
            var defaultStartRow = 7;

            var preResult = new List<BilutenExcelItem>();
            var result = new List<RegionStatisticsDto>();
            var bilutenName = "Бюллетень";
            var yearPos = path.IndexOf(bilutenName) + bilutenName.Length + 1;
            var year = 0;
            var parseValue = 0;

            if (!int.TryParse(path.Substring(yearPos, 4), out year))
                return result;

            using (var package = new ExcelPackage(path))
            {
                var worksheet = package.Workbook.Worksheets[workSheetName];
                if (worksheet.Cells[defaultStartRow, defaultColumnNumber]?.Value == null)
                {
                    defaultColumnNumber = 2;
                    defaultStartRow = 10;
                }
                else
                {
                    defaultStartRow = 7;
                    defaultColumnNumber = 1;
                }
                for (int rowNumber = defaultStartRow; !string.IsNullOrWhiteSpace(worksheet.Cells[rowNumber, defaultColumnNumber].Text); rowNumber++)
                {
                    if (worksheet.Cells[rowNumber, defaultColumnNumber]?.Text?.Contains('-') == true || !int.TryParse(worksheet.Cells[rowNumber, defaultColumnNumber]?.Text, out parseValue))
                        continue;

                    var item = new BilutenExcelItem();
                    item.Year = year;
                    foreach (var property in typeof(BilutenExcelItem).GetProperties())
                    {
                        var attribute = property.GetCustomAttribute<ExcelAttribute>();

                        if (attribute != null)
                            property.SetValue(item, ConvertToInt(worksheet.Cells[rowNumber, attribute.ColumnNumber + columnOffset].Value));
                    }

                    preResult.Add(item);
                }
            }

            return ConvertFromBilutenToDto(preResult);
        }

        public List<BirthRateEntity> ParseBirthRateBiluten(string path)
        {
            const string BirthRateWorksheetName = "2.1.1";
            const int FirstRow = 6;
            const int LastRow = 101;
            var result = new List<BirthRateEntity>();

            using(var package = new ExcelPackage(path))
            {
                var birthRateWorksheet = package.Workbook.Worksheets[BirthRateWorksheetName];
                if (birthRateWorksheet is null)
                    return null;

                for (int currentRow = FirstRow; currentRow <= LastRow; currentRow++)
                {
                    var currentRegion = IsKnowRegion(birthRateWorksheet.Cells[currentRow, 1]?.Value?.ToString());
                    if (currentRegion == null)
                        continue;

                    for (int birthRateColumn = 2; birthRateColumn <= 23; birthRateColumn++)
                        result.Add(new BirthRateEntity
                        {
                            Year = 2024 + (birthRateColumn - 2),
                            BirthRate = (double)ConvertToDouble(birthRateWorksheet.Cells[currentRow, birthRateColumn].Value) * 1000,
                            RegionNumber = currentRegion.Number
                        });
                }
            }

            return result;
        }

        public void FillForecastFile(string path, string regionName, string forecastName, Dictionary<int, List<RegionStatisticsDto>> forecastDictionary, 
            Dictionary<int, List<RegionStatisticsDto>>? retrospectivePermanentData = null, Dictionary<int, List<RegionStatisticsDto>>? retrospectiveEconomyData = null)
        {
            int startRow = 4;
            int startColumn = 3;
            string permanentMessage = "Численность постоянного населения - {0} по возрасту на 1 января (человек)";
            string economyMessage = "Численность занятого населения - {0} по возрасту на 1 января (человек)";
            string message = "";
            var headers = new List<string>() { "Возраст", "СФ" };
            var years = forecastDictionary
                .Select(x => x.Key)
                .ToList();
            years.Sort();
            var rowsCount = 0;
            headers.AddRange(years.Select(x => x.ToString()));

            using (var package = new ExcelPackage())
            {
                var maleWorkSheet = package.Workbook.Worksheets.Add("Мужчины");
                var femaleWorkSheet = package.Workbook.Worksheets.Add("Женщины");
                var allWorkSheet = package.Workbook.Worksheets.Add("Все");

                if (retrospectivePermanentData is not null)
                {
                    message = permanentMessage;
                    AddRangeDictionary(forecastDictionary, retrospectivePermanentData);
                }

                if (retrospectiveEconomyData is not null)
                {
                    message = economyMessage;  
                    AddRangeDictionary(forecastDictionary, retrospectiveEconomyData);
                }

                FillFile(package, startRow, startColumn, forecastDictionary, regionName, maleWorkSheet, femaleWorkSheet, allWorkSheet);

                maleWorkSheet.Cells.AutoFitColumns();
                femaleWorkSheet.Cells.AutoFitColumns();
                allWorkSheet.Cells.AutoFitColumns();
                maleWorkSheet.Cells[startRow - 3, startColumn - 2].Value = string.Format(message, "мужчин");
                femaleWorkSheet.Cells[startRow - 3, startColumn - 2].Value = string.Format(message, "женщин");
                allWorkSheet.Cells[startRow - 3, startColumn - 2].Value = string.Format(message, "всех");
                package.SaveAs(new FileInfo(path + $"/{forecastName}.xlsx"));
            }
        }

        private void AddRangeDictionary(Dictionary<int, List<RegionStatisticsDto>> forecastDictionary,
            Dictionary<int, List<RegionStatisticsDto>> additionalData)
        {
            foreach(var data in additionalData)
                forecastDictionary.TryAdd(data.Key, data.Value);
        }

        private int FillFile(ExcelPackage package, int startRow, int startColumn, Dictionary<int, List<RegionStatisticsDto>> dataDictionary, string regionName,
            ExcelWorksheet maleWorkSheet, ExcelWorksheet femaleWorkSheet, ExcelWorksheet allWorkSheet, int startHeadersColumn = 0)
        {
            var rowsCount = 0;
            var headers = new List<string>() { "Возраст", "СФ" };
            var years = dataDictionary
                .Select(x => x.Key)
                .ToList();
            years.Sort();
            headers.AddRange(years.Select(x => x.ToString()));
            maleWorkSheet.Cells[startRow - 3, startColumn - 2].Style.Font.Bold = true;
            femaleWorkSheet.Cells[startRow - 3, startColumn - 2].Style.Font.Bold = true;
            allWorkSheet.Cells[startRow - 3, startColumn - 2].Style.Font.Bold = true;
            SetHeaders(maleWorkSheet, headers, startRow - 1, startHeadersColumn);
            SetHeaders(femaleWorkSheet, headers, startRow - 1, startHeadersColumn);
            SetHeaders(allWorkSheet, headers, startRow - 1, startHeadersColumn);
            var currentColumn = startColumn;
            var maxAge = 0;
            foreach (var data in dataDictionary)
                maxAge = Math.Max(data.Value.Where(x => x.SummaryByYearSmoothed > 0).Select(x => x.Age).Max(), maxAge);

            for (int age = 0; age <= maxAge; age++)
            {
                maleWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Value = age;
                femaleWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Value = age;
                allWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Value = age;
                maleWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Style.Font.Bold = true;
                femaleWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Style.Font.Bold = true;
                allWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Style.Font.Bold = true;
                maleWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                femaleWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                allWorkSheet.Cells[age + startRow, 1 + startHeadersColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                maleWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Value = regionName;
                femaleWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Value = regionName;
                allWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Value = regionName;
                maleWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Style.Font.Bold = true;
                femaleWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Style.Font.Bold = true;
                allWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Style.Font.Bold = true;
                maleWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                femaleWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                allWorkSheet.Cells[age + startRow, 2 + startHeadersColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                rowsCount++;
            }

            foreach (var year in years)
            {
                if (!dataDictionary.TryGetValue(year, out var values))
                    continue;

                var currentRow = startRow;
                var valuesByAge = values.GroupBy(x => x.Age);
                var summaryMalesCount = 0.0;
                var summaryFemalesCount = 0.0;
                var summaryAllCount = 0.0;
                foreach (var valueByAge in valuesByAge)
                {
                    var allValue = 0.0;
                    foreach (var value in valueByAge.ToList())
                    {
                        if (value.Gender == Gender.Male)
                        {
                            maleWorkSheet.Cells[currentRow, currentColumn].Value = GetRoundedValue(value.SummaryByYearSmoothed);
                            maleWorkSheet.Cells[currentRow, currentColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            summaryMalesCount += value.SummaryByYearSmoothed;
                        }
                        else
                        {
                            femaleWorkSheet.Cells[currentRow, currentColumn].Value = GetRoundedValue(value.SummaryByYearSmoothed);
                            femaleWorkSheet.Cells[currentRow, currentColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            summaryFemalesCount += value.SummaryByYearSmoothed;
                        }
                        allValue += value.SummaryByYearSmoothed;
                    }
                    allWorkSheet.Cells[currentRow, currentColumn].Value = GetRoundedValue(allValue);
                    summaryAllCount += allValue;
                    allWorkSheet.Cells[currentRow, currentColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    currentRow++;
                }
                maleWorkSheet.Cells[currentRow + 1, currentColumn].Value = GetRoundedValue(summaryMalesCount);
                femaleWorkSheet.Cells[currentRow + 1, currentColumn].Value = GetRoundedValue(summaryFemalesCount);
                allWorkSheet.Cells[currentRow + 1, currentColumn].Value = GetRoundedValue(summaryAllCount);
                currentColumn++;
            }

            return rowsCount;
        }

        private string GetRoundedValue(double value)
        {
            return Math.Round(value, 0).ToString();
        }

        private void SetHeaders(ExcelWorksheet worksheet, List<string> headers, int row, int startColumn = 0)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cells[row, i + startColumn + 1].Value = headers[i];
                worksheet.Cells[row, i + startColumn + 1].Style.Font.Bold = true;
                worksheet.Cells[row, i + startColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
        }

        public List<RegionExcelItem> GetBulitenWorksheets(string path, ref BilutenType type)
        {
            var result = new List<RegionExcelItem>();
            using (var package = new ExcelPackage(path))
            {
                var worksheets = package.Workbook.Worksheets;

                foreach(var worksheet in worksheets)
                {
                    var currentRegion = TryGetCurrentRegion(worksheet, ref type);

                    if (currentRegion == null)
                        continue;

                    if (currentRegion != null)
                        result.Add(new RegionExcelItem
                        {
                            Number = currentRegion.Number,
                            WorkSheetName = worksheet.Name
                        });
                }
            }

            return result;
        }

        private RegionInfoEntity IsKnowRegion(string value)
        {
            return Regions.FirstOrDefault(region => region.Name?.ToLowerInvariant().Trim() == value?.ToLowerInvariant().Trim());
        }

        private RegionInfoEntity TryGetCurrentRegion(ExcelWorksheet worksheet, ref BilutenType type)
        {
            var regionName = worksheet.Cells[4, 2]?.Value?.ToString()?.Trim()?.ToLower();
            var currentRegion = new RegionInfoEntity();

            if (regionName?.Contains('(') == false)
            {
                currentRegion = Regions.FirstOrDefault(region => region.Name.Trim().ToLower() == regionName);
                if(currentRegion != null)
                {
                    type = BilutenType.Old;
                    return currentRegion;
                }
            }

            regionName = worksheet.Cells[2, 1]?.Value?.ToString()?.Trim()?.ToLower();
            currentRegion = Regions.FirstOrDefault(region => region.Name.Trim().ToLower() == regionName);

            if (currentRegion != null)
                type = BilutenType.New;

            return currentRegion;
        }

        private object ConvertToInt(object value)
        {
            var result = 0;
            int.TryParse(value.ToString(), out result);

            return result;
        }

        private object ConvertToDouble(object value)
        {
            var result = 0.0;
            double.TryParse(value.ToString(), out result);

            return result;
        }

        private List<RegionStatisticsDto> ConvertFromBilutenToDto(List<BilutenExcelItem> items)
        {
            var result = new List<RegionStatisticsDto>();

            foreach(var item in items)
            {
                result.Add(new RegionStatisticsDto
                {
                    Age = item.Age,
                    Year = item.Year,
                    Gender = Gender.Male,
                    SummaryByYear = item.MalesCount
                });

                result.Add(new RegionStatisticsDto
                {
                    Age = item.Age,
                    Year = item.Year,
                    Gender = Gender.Female,
                    SummaryByYear = item.FemalesCount
                });
            }

            return result;
        }

        private void CalculateByAge(List<RegionStatisticsDto> result, IEnumerable<RegionStatisticsExcelItem> peoples)
        {
            var grouppedByAge = peoples.GroupBy(x => x.Age);
            foreach (var age in grouppedByAge)
            {
                var years = age.Select(x => x.TargetYear).Distinct();
                foreach (var year in years)
                {
                    var ageByYear = age.Where(x => x.TargetYear == year);
                    var summaryInAge = ageByYear.Sum(x => x.Count);
                    var first = ageByYear.First();

                    result.Add(ConvertToDto(first, summaryInAge));
                }
            }
        }

        private RegionStatisticsDto ConvertToDto(RegionStatisticsExcelItem item, int summaryByYear)
        {
            return new RegionStatisticsDto
            {
                Age = (int)item.Age,
                Gender = DetermineGender(item.Gender),
                SummaryByYear = summaryByYear,
                Year = item.TargetYear
            };
        }

        private Gender DetermineGender(string value)
        {
            if (value.ToLower().Contains("муж"))
                return Gender.Male;
            else
                return Gender.Female;
        }

        public List<ExcelWorksheet> ReadPackage(string path)
        {
            using (var package = new ExcelPackage(path)) 
            {
                return package.Workbook.Worksheets
                    .Where(worksheet => worksheet.Hidden == eWorkSheetHidden.Visible)
                    .ToList();       
            }
        }
    }
}
