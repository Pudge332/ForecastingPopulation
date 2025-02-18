using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Database.Models
{
    public class BasePopulationEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Номер региона 
        /// </summary>
        public int RegionNumber { get; set; }
        /// <summary>
        /// Численность занятого в экономике населения
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Пол
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Год
        /// </summary>
        public int Year { get; set; }
    }
}
