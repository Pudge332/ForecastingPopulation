using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Models.Dto
{
    public class RegionCoefficentDto
    {
        public int Year { get; set; }
        public double Age { get; set; }
        public double Coefficent { get; set; }
        public Gender Gender { get; set; }
    }
}
