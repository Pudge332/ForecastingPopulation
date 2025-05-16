using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Models.Attributes;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;
using ForecastingWorkingPopulation.Models.Excel;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Reflection;
using static OfficeOpenXml.ExcelErrorValue;

namespace ForecastingWorkingPopulation.Infrastructure.Excel
{
    public class ExcelParser : IExcelParser
    {
        private List<RegionInfoEntity> Regions = RegionRepository.GetRegions();
        public List<RegionStatisticsDto> Parse(string path, int startRowNumber, List<int> years, int endColumnNumber = 10)
        {
            //string path = "C:\\Excels\\Республика Карелия.xlsx";
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
            var defaultColumnNumber = 2;
            var defaultStartRow = 10;

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

        public void FillForecastFile(string path, string regionName, string forecastName,Dictionary<int, List<RegionStatisticsDto>> forecastDictionary)
        {
            const int StartRow = 4;
            const int StartColumn = 3;
            const string Message = "Численность постоянного населения - {0} по возрасту на 1 января (человек)";

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
                maleWorkSheet.Cells[1, 1].Style.Font.Bold = true;
                femaleWorkSheet.Cells[1, 1].Style.Font.Bold = true;
                allWorkSheet.Cells[1, 1].Style.Font.Bold = true;
                SetHeaders(maleWorkSheet, headers, StartRow - 1);
                SetHeaders(femaleWorkSheet, headers, StartRow - 1);
                SetHeaders(allWorkSheet, headers, StartRow - 1); 
                var currentColumn = StartColumn;
                for(int age = 0; age < forecastDictionary.Last().Value.Select(x => x.Age).Max(); age++)
                {
                    maleWorkSheet.Cells[age + 4, 1].Value = age;
                    femaleWorkSheet.Cells[age + 4, 1].Value = age;
                    allWorkSheet.Cells[age + 4, 1].Value = age;
                    maleWorkSheet.Cells[age + 4, 1].Style.Font.Bold = true;
                    femaleWorkSheet.Cells[age + 4, 1].Style.Font.Bold = true;
                    allWorkSheet.Cells[age + 4, 1].Style.Font.Bold = true;
                    maleWorkSheet.Cells[age + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    femaleWorkSheet.Cells[age + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    allWorkSheet.Cells[age + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    maleWorkSheet.Cells[age + 4, 2].Value = regionName;
                    femaleWorkSheet.Cells[age + 4, 2].Value = regionName;
                    allWorkSheet.Cells[age + 4, 2].Value = regionName;
                    maleWorkSheet.Cells[age + 4, 2].Style.Font.Bold = true;
                    femaleWorkSheet.Cells[age + 4, 2].Style.Font.Bold = true;
                    allWorkSheet.Cells[age + 4, 2].Style.Font.Bold = true;
                    maleWorkSheet.Cells[age + 4, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    femaleWorkSheet.Cells[age + 4, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    allWorkSheet.Cells[age + 4, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    rowsCount++;
                }

                foreach (var year in years)
                {
                    if (!forecastDictionary.TryGetValue(year, out var values))
                        continue;

                    var currentRow = StartRow;
                    var valuesByAge = values.GroupBy(x => x.Age);
                    var summaryMalesCount = 0.0;
                    var summaryFemalesCount = 0.0;
                    var summaryAllCount = 0.0;
                    foreach (var valueByAge in valuesByAge)
                    {
                        if (currentRow - 4 >= rowsCount)
                            continue;

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

                maleWorkSheet.Cells.AutoFitColumns();
                femaleWorkSheet.Cells.AutoFitColumns();
                allWorkSheet.Cells.AutoFitColumns();
                maleWorkSheet.Cells[1, 1].Value = string.Format(Message, "мужчин");
                femaleWorkSheet.Cells[1, 1].Value = string.Format(Message, "женщин");
                allWorkSheet.Cells[1, 1].Value = string.Format(Message, "всех");
                package.SaveAs(new FileInfo(path + $"/{forecastName}.xlsx"));
            }
        }

        public void CreateForecastFile(string path, Dictionary<int, List<RegionStatisticsDto>> forecastDictionary)
        {
            const int StartRow = 3;
            const int ColumnOffset = 4;
            var forecastName = $"Прогноз до {forecastDictionary.Last().Key} года";
            var headers = new List<string>() { "Все", "Мужчины", "Женщины" };
            var yearHeaders = forecastDictionary
                .Select(x => x.Key.ToString())
                .ToList();
            var column = 2;
            using(var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(forecastName);
                worksheet.Cells[2, 1].Value = "Возраст";
                SetYearHeader(worksheet, yearHeaders, 1);
                for (int ageRow = StartRow; ageRow < 90; ageRow++)
                    worksheet.Cells[ageRow, 1].Value = (ageRow - 3).ToString();

                foreach (var oneYearForecast in forecastDictionary)
                {
                    var row = StartRow;
                    var counter = 0;
                    foreach(var forecastData in oneYearForecast.Value)
                    {
                        counter++;
                        worksheet.Cells[row, column].Value = GetRoundedValue(oneYearForecast.Value
                            .Where(dto => dto.Age == forecastData.Age)
                            .Sum(dto => dto.SummaryByYearSmoothed));

                        if(forecastData.Gender == Gender.Male)
                            worksheet.Cells[row, column + 1].Value = GetRoundedValue(forecastData.SummaryByYearSmoothed);
                        else
                            worksheet.Cells[row, column + 2].Value = GetRoundedValue(forecastData.SummaryByYearSmoothed);

                        if (counter % 2 == 0)
                            row++;
                    }
                    SetHeaders(worksheet, headers, 2, column - 1);
                    column += ColumnOffset;
                }
                package.SaveAs(new FileInfo(path + $"/{forecastName}.xlsx"));
            }
        }

        private string GetRoundedValue(double value)
        {
            return Math.Round(value, 2).ToString();
        }

        private void SetYearHeader(ExcelWorksheet worksheet, List<string> headers, int row)
        {
            var column = 3;
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cells[row, column].Value = headers[i];
                column += 4;
            }
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
