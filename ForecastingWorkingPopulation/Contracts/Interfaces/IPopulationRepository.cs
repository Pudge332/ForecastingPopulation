using ForecastingWorkingPopulation.Database.Models;
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

        /// <summary>
        /// Получает настройки коэффициентов для указанного региона
        /// </summary>
        /// <param name="regionNumber">Номер региона</param>
        /// <returns>Настройки коэффициентов или null, если настройки не найдены</returns>
        public RegionCoefficientSettingsEntity GetRegionCoefficientSettings(int regionNumber);

        /// <summary>
        /// Сохраняет настройки коэффициентов для указанного региона
        /// </summary>
        /// <param name="settings">Настройки коэффициентов</param>
        public void SaveRegionCoefficientSettings(RegionCoefficientSettingsEntity settings);

        /// <summary>
        /// Получает настройки главной формы для указанного региона
        /// </summary>
        /// <param name="regionNumber">Номер региона</param>
        /// <returns>Настройки главной формы или null, если настройки не найдены</returns>
        public RegionMainFormSettingsEntity GetRegionMainFormSettings(int regionNumber);

        /// <summary>
        /// Сохраняет настройки главной формы для указанного региона
        /// </summary>
        /// <param name="settings">Настройки главной формы</param>
        public void SaveRegionMainFormSettings(RegionMainFormSettingsEntity settings);
    }
}
