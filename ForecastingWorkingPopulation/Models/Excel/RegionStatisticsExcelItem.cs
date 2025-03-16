using ForecastingWorkingPopulation.Models.Attributes;
using System.ComponentModel;

namespace ForecastingWorkingPopulation.Models.Excel
{
    public class RegionStatisticsExcelItem : BaseRegionStatisticExcelItem
    {
        /// <summary>
        /// Возраст
        /// </summary>
        [ExcelAttribute(4, false, typeof(int))]
        public double Age { get; set; }
        /// <summary>
        /// Пол
        /// </summary>
        [ExcelAttribute(5, false, typeof(string))]
        public string Gender { get; set; }
        [ExcelAttribute(6, true, typeof(double))]
        public double CountByYearValue { get; set; }
        /// <summary>
        /// Количество в необходимом нам формате
        /// </summary>
        public int Count { get => (int)Math.Round(CountByYearValue * 1000); }
    }
}
