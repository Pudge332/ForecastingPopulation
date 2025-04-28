using System.Collections.Generic;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Infrastructure
{
    public sealed class CalculationStorage
    {
        private static readonly CalculationStorage instance = new CalculationStorage();
        private List<RegionCoefficentDto> lifeExpectancyDataMale;
        private List<RegionCoefficentDto> lifeExpectancyDataFemale;
        private List<RegionInEconomyLevelDto> inEconomyLevelData;
        private List<RegionInEconomyLevelDto> inEconomyLevelDataSmoothed;
        private Dictionary<int, List<RegionStatisticsDto>> economyEmploedRegionStatisticsData;
        private Dictionary<int, List<RegionStatisticsDto>> economyEmploedRegionStatisticsDataSmoothed;
        private Dictionary<int, List<RegionStatisticsDto>> permanentPopulationdRegionStatisticsData;
        private Dictionary<int, List<RegionStatisticsDto>> permanentPopulationStatisticsDataSmoothed;
        public Dictionary<int, List<RegionStatisticsDto>> PermanentPopulationForecast { get; set; }
        public int currentRegion;

        private List<int> availableYears = new List<int>();

        private CalculationStorage()
        {
            lifeExpectancyDataMale = new List<RegionCoefficentDto>();
            lifeExpectancyDataFemale = new List<RegionCoefficentDto>();
            inEconomyLevelData = new List<RegionInEconomyLevelDto>();
            economyEmploedRegionStatisticsData = new Dictionary<int, List<RegionStatisticsDto>>();
            economyEmploedRegionStatisticsDataSmoothed = new Dictionary<int, List<RegionStatisticsDto>>();
            permanentPopulationdRegionStatisticsData = new Dictionary<int, List<RegionStatisticsDto>>();
            permanentPopulationStatisticsDataSmoothed = new Dictionary<int, List<RegionStatisticsDto>>();
            PermanentPopulationForecast = new Dictionary<int, List<RegionStatisticsDto>>();
        }

        public static CalculationStorage Instance
        {
            get { return instance; }
        }

        public void StoreLifeExpectancyCalculation(List<RegionCoefficentDto> data, Gender gender)
        {
            if (gender == Gender.Male)
            {
                lifeExpectancyDataMale = new List<RegionCoefficentDto>(data);
            }
            else
            {
                lifeExpectancyDataFemale = new List<RegionCoefficentDto>(data);
            }
        }

        public List<RegionCoefficentDto> GetLifeExpectancyDataMale()
        {
            return new List<RegionCoefficentDto>(lifeExpectancyDataMale);
        }

        public List<RegionCoefficentDto> GetLifeExpectancyDataFemale()
        {
            return new List<RegionCoefficentDto>(lifeExpectancyDataFemale);
        }

        public List<RegionCoefficentDto> GetLifeExpectancyData(Gender gender)
        {
            return gender == Gender.Male ? GetLifeExpectancyDataMale() : GetLifeExpectancyDataFemale();
        }

        public List<RegionCoefficentDto> GetLifeExpectancyData()
        {
            var combinedData = new List<RegionCoefficentDto>();
            combinedData.AddRange(lifeExpectancyDataMale);
            combinedData.AddRange(lifeExpectancyDataFemale);
            return combinedData;
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

        public void StoreEconomyEmploedRegionStatistics(int year, List<RegionStatisticsDto> data)
        {
            if (economyEmploedRegionStatisticsData.ContainsKey(year))
            {
                economyEmploedRegionStatisticsData.Remove(year);
                economyEmploedRegionStatisticsData.Add(year, data);
            }
            else
            {
                economyEmploedRegionStatisticsData.Add(year, data);
            }
        }

        public void StorePermanentPopulationRegionStatistics(int year, List<RegionStatisticsDto> data)
        {
            if (permanentPopulationdRegionStatisticsData.ContainsKey(year))
            {
                permanentPopulationdRegionStatisticsData.Remove(year);
                permanentPopulationdRegionStatisticsData.Add(year, data);
            }
            else
            {
                permanentPopulationdRegionStatisticsData.Add(year, data);
            }
        }

        public Dictionary<int, List<RegionStatisticsDto>> GetEconomyEmploedRegionStatisticsData()
        {
            return economyEmploedRegionStatisticsData;
        }

        public List<RegionStatisticsDto> GetEconomyEmploedRegionStatisticsValues()
        {
            return economyEmploedRegionStatisticsData.Values.SelectMany(x => x).ToList();
        }

        public void StoreEconomyEmploedRegionStatisticsSmoothed(int year, List<RegionStatisticsDto> data)
        {
            if (economyEmploedRegionStatisticsDataSmoothed.ContainsKey(year))
            {
                economyEmploedRegionStatisticsDataSmoothed.Remove(year);
                economyEmploedRegionStatisticsDataSmoothed.Add(year, data);
            }
            else
            {
                economyEmploedRegionStatisticsDataSmoothed.Add(year, data);
            }
        }

        public Dictionary<int, List<RegionStatisticsDto>> GetEconomyEmploedRegionStatisticsDataSmoothed()
        {
            return economyEmploedRegionStatisticsDataSmoothed;
        }

        public List<RegionStatisticsDto> GetEconomyEmploedRegionStatisticsValuesSmoothed()
        {
            return economyEmploedRegionStatisticsDataSmoothed.Values.SelectMany(x => x).ToList();
        }

        public Dictionary<int, List<RegionStatisticsDto>> GetPermanentPopulationRegionStatisticsData()
        {
            return permanentPopulationdRegionStatisticsData;
        }

        public List<RegionStatisticsDto> GetPermanentPopulationRegionStatisticsValues()
        {
            return permanentPopulationdRegionStatisticsData.Values.SelectMany(x => x).ToList();
        }

        public void StorePermanentPopulationStatisticsSmoothed(int year, List<RegionStatisticsDto> data)
        {
            if (permanentPopulationStatisticsDataSmoothed.ContainsKey(year))
            {
                permanentPopulationStatisticsDataSmoothed.Remove(year);
                permanentPopulationStatisticsDataSmoothed.Add(year, data);
            }
            else
            {
                permanentPopulationStatisticsDataSmoothed.Add(year, data);
            }
        }

        public Dictionary<int, List<RegionStatisticsDto>> GetEconomyPermanentPopulationStatisticsDataSmoothed()
        {
            return permanentPopulationStatisticsDataSmoothed;
        }

        public List<RegionStatisticsDto> GetPermanentPopulationStatisticsValuesSmoothed()
        {
            return permanentPopulationStatisticsDataSmoothed.Values.SelectMany(x => x).ToList();
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
                economyEmploedRegionStatisticsData.Clear();

            CurrentRegion = value;
        }
    }
}
