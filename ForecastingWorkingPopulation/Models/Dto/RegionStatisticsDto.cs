using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Models.Dto
{
    public class RegionStatisticsDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Пол
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Колличество за год
        /// </summary>
        public int SummaryByYear { get; set; }
        /// <summary>
        /// Год
        /// </summary>
        public int Year { get; set; }
    }
}
