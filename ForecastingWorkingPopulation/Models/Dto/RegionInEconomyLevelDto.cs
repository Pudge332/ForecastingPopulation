using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Models.Dto
{
    public class RegionInEconomyLevelDto
    {
        public int Year { get; set; }
        public Gender Gender { get; set; }
        public double Age { get; set; }
        public double Level { get; set; }
    }
}
