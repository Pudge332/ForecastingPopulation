﻿using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Models.Dto;

namespace ForecastingWorkingPopulation.Contracts.Interfaces
{
    public interface IPopulationRepository
    {
        public List<RegionStatisticsDto> GetPopulationInRegion(int regionNumber);
        public List<RegionStatisticsDto> GetEconomyEmployedInRegion(int regionNumber);
        public List<RegionInfoEntity> GetAllRegions();
        public void SaveEmployedEconomyEntyties(int regionNumber, List<RegionStatisticsDto> dtos);
        public void SavePermanentPopulationEntyties(int regionNumber, List<RegionStatisticsDto> dtos);
    }
}
