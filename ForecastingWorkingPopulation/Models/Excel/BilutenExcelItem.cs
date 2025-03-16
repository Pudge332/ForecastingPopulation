using ForecastingWorkingPopulation.Models.Attributes;

namespace ForecastingWorkingPopulation.Models.Excel
{
    public class BilutenExcelItem 
    {
        /// <summary>
        /// Год
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Возраст
        /// </summary>
        [ExcelAttribute(1, false, typeof(int))]
        public int Age { get; set; }
        /// <summary>
        /// Количество мужчин
        /// </summary>
        [ExcelAttribute(3, false, typeof(int))]
        public int MalesCount { get; set; }
        /// <summary>
        /// Количество женщин
        /// </summary>
        [ExcelAttribute(4, false, typeof(int))]
        public int FemalesCount { get; set; }
    }
}
