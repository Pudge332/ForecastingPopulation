using System.Collections.Generic;
using ForecastingWorkingPopulation.Models.Dto;

namespace ForecastingWorkingPopulation.Infrastructure
{
    public sealed class CalculationStorage
    {
        private static readonly CalculationStorage instance = new CalculationStorage();
        private List<RegionCoefficentDto> lifeExpectancyData;
        private List<RegionInEconomyLevelDto> inEconomyLevelData;
        private List<RegionInEconomyLevelDto> inEconomyLevelDataSmoothed;
        private Dictionary<int, List<RegionStatisticsDto>> regionStatisticsData;
        private Dictionary<int, List<RegionStatisticsDto>> regionStatisticsDataSmoothed;
        private int currentRegion;

        private List<int> availableYears = new List<int>();

        private CalculationStorage()
        {
            lifeExpectancyData = new List<RegionCoefficentDto>();
            inEconomyLevelData = new List<RegionInEconomyLevelDto>();
            regionStatisticsData = new Dictionary<int, List<RegionStatisticsDto>>();
            regionStatisticsDataSmoothed = new Dictionary<int, List<RegionStatisticsDto>>();
        }

        public static CalculationStorage Instance
        {
            get { return instance; }
        }

        public void StoreLifeExpectancyCalculation(List<RegionCoefficentDto> data)
        {
            lifeExpectancyData = data;
        }

        public List<RegionCoefficentDto> GetLifeExpectancyData()
        {
            return lifeExpectancyData;
        }

        public void StoreInEconomyLevel(List<RegionInEconomyLevelDto> data)
        {
            inEconomyLevelData = new(data);
        }

        public List<RegionInEconomyLevelDto> GetInEconomyLevel() =>
            inEconomyLevelData;

        public void StoreInEconomySmoothedLevel(List<RegionInEconomyLevelDto> data)
        {
            inEconomyLevelDataSmoothed = new(data);
        }

        public List<RegionInEconomyLevelDto> GetInEconomyLevelSmoothed() =>
            inEconomyLevelDataSmoothed;

        public void StoreRegionStatistics(int year, List<RegionStatisticsDto> data)
        {
            if (regionStatisticsData.ContainsKey(year))
            {
                regionStatisticsData[year] = data;
            }
            else
            {
                regionStatisticsData.Add(year, data);
            }
        }

        public Dictionary<int, List<RegionStatisticsDto>> GetRegionStatisticsData()
        {
            return regionStatisticsData;
        }

        public List<RegionStatisticsDto> GetRegionStatisticsValues()
        {
            return regionStatisticsData.Values.SelectMany(x => x).ToList();
        }

        public void StoreRegionStatisticsSmoothed(int year, List<RegionStatisticsDto> data)
        {
            if (regionStatisticsDataSmoothed.ContainsKey(year))
            {
                regionStatisticsDataSmoothed[year] = data;
            }
            else
            {
                regionStatisticsDataSmoothed.Add(year, data);
            }
        }

        public Dictionary<int, List<RegionStatisticsDto>> GetRegionStatisticsDataSmoothed()
        {
            return regionStatisticsDataSmoothed;
        }

        public List<RegionStatisticsDto> GetRegionStatisticsValuesSmoothed()
        {
            return regionStatisticsDataSmoothed.Values.SelectMany(x => x).ToList();
        }

        public List<int> GetAvailableYears()
        {
            return availableYears;
        }

        public void StoreAvailableYears(List<int> years)
        {
            availableYears = years;
        }

        public int CurrentRegion
        {
            get { return currentRegion; }
            set { currentRegion = value; }
        }

        private void RegionNumberChanged(int value)
        {
            if (CurrentRegion != value)
                regionStatisticsData.Clear();

            CurrentRegion = value;
        }
    }
}
