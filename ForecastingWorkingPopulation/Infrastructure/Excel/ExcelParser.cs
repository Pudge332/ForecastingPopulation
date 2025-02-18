using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Models.Attributes;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;
using ForecastingWorkingPopulation.Models.Excel;
using OfficeOpenXml;
using System.CodeDom;
using System.ComponentModel;
using System.Reflection;

namespace ForecastingWorkingPopulation.Infrastructure.Excel
{
    public class ExcelParser : IExcelParser
    {
        public List<RegionStatisticsDto> Parse(ExcelWorksheet worksheet, int startRowNumber, List<int> years, int endColumnNumber = 10)
        {
            string path = "C:\\Excels\\Республика Карелия.xlsx";
            var preResult = new List<RegionStatisticsExcelItem>();
            var result = new List<RegionStatisticsDto>();

            using (var package = new ExcelPackage(path))
            {
                worksheet = package.Workbook.Worksheets[0];

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
