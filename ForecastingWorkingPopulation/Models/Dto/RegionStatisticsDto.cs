using ForecastingWorkingPopulation.Models.Enums;
using System.Reflection.Metadata.Ecma335;

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
        /// Сглаженное значение
        /// </summary>
        private double _summaryByYearSmoothed;
        public double SummaryByYearSmoothed 
        { 
            get 
            { 
                if (_summaryByYearSmoothed > 0) 
                    return _summaryByYearSmoothed; 

                return SummaryByYear; 
            } 
            set => _summaryByYearSmoothed = value; 
        }
        /// <summary>
        /// Год
        /// </summary>
        public int Year { get; set; }
    }
}
