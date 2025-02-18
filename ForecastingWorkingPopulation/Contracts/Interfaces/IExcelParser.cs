using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Excel;
using OfficeOpenXml;

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
        public List<RegionStatisticsDto> Parse(ExcelWorksheet worksheet, int startRowNumber, List<int> years, int endColumnNumber = 10); 
    }
}
