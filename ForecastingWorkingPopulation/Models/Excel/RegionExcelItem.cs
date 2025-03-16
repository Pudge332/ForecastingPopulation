namespace ForecastingWorkingPopulation.Models.Excel
{
    public class RegionExcelItem
    {
        /// <summary>
        /// Номер региона
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Название страницы на которой содержится инфомрация о регионе
        /// </summary>
        public string WorkSheetName { get; set; }
    }
}
