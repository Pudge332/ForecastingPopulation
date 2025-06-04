using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Infrastructure
{
    public class ChartDataService
    {
        private readonly SmoothingCalculator _smoothingCalculator;

        public ChartDataService()
        {
            _smoothingCalculator = new SmoothingCalculator();
        }

        public List<RegionStatisticsDto> PrepareChartData(List<RegionStatisticsDto> dtos, GenderComboBox gender, int windowSize, int smoothingCount, bool useSmoothing)
        {
            var result = new List<RegionStatisticsDto>();
            var groupedByYear = dtos
                .GroupBy(d => d.Year)
                .OrderBy(g => g.Key);

            foreach (var yearGroup in groupedByYear)
            {
                var selectedData = SelectByGender(gender, yearGroup).ToList();
                if (gender == GenderComboBox.All)
                {
                    result.AddRange(_smoothingCalculator.SmoothingValuesDto(SelectByGender(GenderComboBox.Males, yearGroup).ToList(), windowSize, SmoothingType.MovingAverageWindow, smoothingCount));
                    result.AddRange(_smoothingCalculator.SmoothingValuesDto(SelectByGender(GenderComboBox.Females, yearGroup).ToList(), windowSize, SmoothingType.MovingAverageWindow, smoothingCount));
                }
                else
                    result.AddRange(_smoothingCalculator.SmoothingValuesDto(selectedData, windowSize, SmoothingType.MovingAverageWindow, smoothingCount));
            }

            return result;
        }

        public IEnumerable<RegionStatisticsDto> SelectByGender(GenderComboBox gender, IEnumerable<RegionStatisticsDto> data)
        {
            switch (gender)
            {
                case GenderComboBox.All:
                    return data;
                case GenderComboBox.Males:
                    return data.Where(d => d.Gender == Gender.Male);
                case GenderComboBox.Females:
                    return data.Where(d => d.Gender == Gender.Female);
                default:
                    return Enumerable.Empty<RegionStatisticsDto>();
            }
        }

        public class SeriesData
        {
            public string SeriesName { get; set; }
            public List<Gender> Genders { get; set; }
            public List<double> XValues { get; set; }
            public List<double> YValues { get; set; }
        }
    }
}
