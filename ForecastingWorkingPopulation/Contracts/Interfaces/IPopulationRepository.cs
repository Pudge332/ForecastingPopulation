using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Models.Dto;

namespace ForecastingWorkingPopulation.Contracts.Interfaces
{
    public interface IPopulationRepository
    {
        public List<RegionStatisticsDto> GetPopulationInRegion(int regionNumber);
        public List<RegionStatisticsDto> GetEconomyEmployedInRegion(int regionNumber);
        public void SaveEntityByRegion<TEntity>(int regionNumber, List<RegionStatisticsDto> dtos) where TEntity : BasePopulationEntity, new();
    }
}
