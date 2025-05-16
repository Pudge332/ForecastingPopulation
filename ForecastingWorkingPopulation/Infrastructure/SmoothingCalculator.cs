using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Infrastructure
{
    public class SmoothingCalculator
    {
        public List<double> SmoothingValues(IEnumerable<RegionStatisticsDto> data, int windowSize, SmoothingType smoothingType, int smoothingCount = 1)
        {
            var result = new List<RegionStatisticsDto>(data);
            foreach (var item in data)
                item.SummaryByYearSmoothed = item.SummaryByYear;

            for (int i = 0; i < smoothingCount; i++)
                result = SetSmoothingType(result, windowSize, smoothingType);

            return result.Select(dto => dto.SummaryByYearSmoothed).ToList();
        }

        public List<RegionStatisticsDto> SmoothingValuesDto(IEnumerable<RegionStatisticsDto> data, int windowSize, SmoothingType smoothingType, int smoothingCount = 1)
        {
            var result = new List<RegionStatisticsDto>(data);
            foreach (var item in data)
                item.SummaryByYearSmoothed = item.SummaryByYear;

            for (int i = 0; i < smoothingCount; i++)
                result = SetSmoothingType(result, windowSize, smoothingType);

            return result;
        }

        public List<RegionInEconomyLevelDto> SmoothingValuesDto(IEnumerable<RegionInEconomyLevelDto> data, int windowSize, SmoothingType smoothingType, int smoothingCount = 1)
        {
            var result = new List<RegionInEconomyLevelDto>(data);
            foreach (var item in data)
                item.Level = item.Level;

            for (int i = 0; i < smoothingCount; i++)
                result = SetSmoothingType(result, windowSize, smoothingType);

            return result;
        }

        private List<RegionStatisticsDto> SetSmoothingType(IEnumerable<RegionStatisticsDto> dtos, int windowSize, SmoothingType smoothingType)
        {
            switch (smoothingType)
            {
                case SmoothingType.MovingAverageWindow:
                    return MovingAverageSmoothing(dtos.ToList(), windowSize);
                case SmoothingType.ExponentialSmoothing:
                    return ExponentialSmoothing(dtos.ToList(), windowSize / 13);
                case SmoothingType.MedianFilter:
                    return MedianFilter(dtos.ToList(), windowSize);
                case SmoothingType.SavitzkyGolayFilter:
                    return SavitzkyGolayFilter(dtos.ToList(), windowSize);
            }

            return null;
        }

        private List<RegionInEconomyLevelDto> SetSmoothingType(IEnumerable<RegionInEconomyLevelDto> dtos, int windowSize, SmoothingType smoothingType)
        {
            switch (smoothingType)
            {
                case SmoothingType.MovingAverageWindow:
                    return MovingAverageSmoothing(dtos.ToList(), windowSize);
            }

            return null;
        }

        public List<RegionStatisticsDto> MovingAverageSmoothing(List<RegionStatisticsDto> data, int windowSize)
        {
            for (int i = 0; i < data.Count - windowSize; i++)
            {
                var smoothingValue = GetSumInRange(data, i, i + windowSize) / windowSize;
                data[i].SummaryByYearSmoothed = smoothingValue;
            }

            return data;
        }

        public List<RegionInEconomyLevelDto> MovingAverageSmoothing(List<RegionInEconomyLevelDto> data, int windowSize)
        {
            for (int i = 0; i < data.Count - windowSize; i++)
            {
                var smoothingValue = GetSumInRange(data, i, i + windowSize) / windowSize;
                data[i].Level = smoothingValue;
            }

            return data;
        }

        public double GetSumInRange(List<RegionStatisticsDto> data, int startIndex, int endIndex)
        {
            var result = 0.0;
            for (int i = startIndex; i < endIndex; i++)
                result += data[i].SummaryByYearSmoothed;

            return result;
        }

        public double GetSumInRange(List<RegionInEconomyLevelDto> data, int startIndex, int endIndex)
        {
            var result = 0.0;
            for (int i = startIndex; i < endIndex; i++)
                result += data[i].Level;

            return result;
        }

        /// <summary>
        /// Экспоненциальное сглаживание (alpha ∈ [0, 1])
        /// </summary>
        public List<RegionStatisticsDto> ExponentialSmoothing(List<RegionStatisticsDto> data, double alpha)
        {
            if (alpha < 0 || alpha > 1)
                throw new ArgumentException("Alpha must be between 0 and 1");

            var smoothed = new List<RegionStatisticsDto>();
            double prev = data[0].SummaryByYearSmoothed; // Инициализация первым значением

            foreach (var value in data)
            {
                prev = alpha * value.SummaryByYearSmoothed + (1 - alpha) * prev;
                smoothed.Add(new RegionStatisticsDto
                {
                    Age = value.Age,
                    SummaryByYear = value.SummaryByYear,
                    SummaryByYearSmoothed = prev,
                    Gender = value.Gender,
                    Year = value.Year
                });
            }

            return smoothed;
        }

        /// <summary>
        /// Медианное сглаживание (windowSize - нечетное число)
        /// </summary>
        public List<RegionStatisticsDto> MedianFilter(List<RegionStatisticsDto> data, int windowSize)
        {
            if (windowSize % 2 == 0)
                windowSize++;

            if (windowSize > data.Count)
                throw new ArgumentException("Window size exceeds data length");

            var smoothed = new List<RegionStatisticsDto>();
            int radius = windowSize / 2;

            for (int i = 0; i < data.Count; i++)
            {
                // Определение границ окна
                int start = Math.Max(0, i - radius);
                int end = Math.Min(data.Count - 1, i + radius);

                // Сбор значений в окне
                List<double> window = new List<double>();
                for (int j = start; j <= end; j++)
                {
                    window.Add(data[j].SummaryByYearSmoothed);
                }

                // Расчет медианы
                window.Sort();
                int mid = window.Count / 2;
                smoothed.Add(new RegionStatisticsDto
                {
                    Age = data[i].Age,
                    SummaryByYear = data[i].SummaryByYear,
                    SummaryByYearSmoothed = window[mid],
                    Gender = data[i].Gender,
                    Year = data[i].Year
                });
            }

            return smoothed;
        }

        /// <summary>
        /// Сглаживание Савицкого-Голея (windowSize - нечетное, polynomialDegree ≤ windowSize)
        /// </summary>
        public List<RegionStatisticsDto> SavitzkyGolayFilter(List<RegionStatisticsDto> data, int windowSize)
        {
            windowSize = 5;
            if (windowSize % 2 == 0)
                windowSize++;

            int radius = windowSize / 2;
            List<RegionStatisticsDto> smoothed = new List<RegionStatisticsDto>();

            double[] coefficients = { -3.0 / 35, 12.0 / 35, 17.0 / 35, 12.0 / 35, -3.0 / 35 };

            for (int i = 0; i < data.Count; i++)
            {
                double sum = 0;
                for (int j = -radius; j <= radius; j++)
                {
                    int index = i + j;
                    if (index < 0) index = 0;
                    if (index >= data.Count) index = data.Count - 1;

                    sum += data[index].SummaryByYearSmoothed * coefficients[j + radius];
                }
                smoothed.Add(new RegionStatisticsDto
                {
                    Age = data[i].Age,
                    SummaryByYear = data[i].SummaryByYear,
                    SummaryByYearSmoothed = sum,
                    Gender = data[i].Gender,
                    Year = data[i].Year
                });
            }

            return smoothed;
        }
    }
}
