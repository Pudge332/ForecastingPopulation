using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Infrastructure
{
    public class ChartDataService
    {
        public List<SeriesData> PrepareChartData(List<RegionStatisticsDto> dtos, GenderComboBox gender, int windowSize, int smoothingCount, bool useSmoothing)
        {
            var result = new List<SeriesData>();
            var groupedByYear = dtos
                .GroupBy(d => d.Year)
                .OrderBy(g => g.Key);

            foreach (var yearGroup in groupedByYear)
            {
                var selectedData = SelectByGender(gender, yearGroup).ToList();
                var xValues = selectedData.Select(d => d.Age).Select(Convert.ToDouble).ToList();
                var yValues = useSmoothing
                    ? ApplySmoothing(selectedData, windowSize, smoothingCount)
                    : selectedData.Select(d => Convert.ToDouble(d.SummaryByYear)).ToList();

                result.Add(new SeriesData
                {
                    SeriesName = $"{yearGroup.Key}",
                    XValues = xValues,
                    YValues = yValues
                });
            }

            return result;
        }
        public IEnumerable<RegionStatisticsDto> SelectByGender(GenderComboBox gender, IEnumerable<RegionStatisticsDto> data)
        {
            switch (gender)
            {
                case GenderComboBox.All:
                    return GetAll(data);
                case GenderComboBox.Males:
                    return data.Where(d => d.Gender == Gender.Male);
                case GenderComboBox.Females:
                    return data.Where(d => d.Gender == Gender.Female);
                default:
                    return Enumerable.Empty<RegionStatisticsDto>();
            }
        }

        public List<double> ApplySmoothing(IEnumerable<RegionStatisticsDto> data, int windowSize, int smoothingCount)
        {
            var result = new List<RegionStatisticsDto>(data);
            foreach (var item in data)
                item.SummaryByYearSmoothed = item.SummaryByYear;

            for (int i = 0; i < smoothingCount; i++)
                result = MovingAverageSmoothing(result, windowSize);

            return result.Select(d => d.SummaryByYearSmoothed).ToList();
        }

        private List<RegionStatisticsDto> MovingAverageSmoothing(List<RegionStatisticsDto> data, int windowSize)
        {
            for (int i = 0; i < data.Count - windowSize; i++)
            {
                data[i].SummaryByYearSmoothed = GetSumInRange(data, i, i + windowSize) / windowSize;
            }
            return data;
        }

        private double GetSumInRange(List<RegionStatisticsDto> data, int start, int end)
        {
            return data.Skip(start).Take(end - start).Sum(d => d.SummaryByYearSmoothed);
        }

        public class SeriesData
        {
            public string SeriesName { get; set; }
            public List<double> XValues { get; set; }
            public List<double> YValues { get; set; }
        }

        private IEnumerable<RegionStatisticsDto> GetAll(IEnumerable<RegionStatisticsDto> data)
        {
            return data
                .GroupBy(d => new { d.Year, d.Age })
                .Select(g => new RegionStatisticsDto
                {
                    Year = g.Key.Year,
                    Age = g.Key.Age,
                    SummaryByYear = g.Sum(x => x.SummaryByYear)
                });
        }
    }
}
