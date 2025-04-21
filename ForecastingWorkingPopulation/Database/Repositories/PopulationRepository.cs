using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Context;
using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Infrastructure.Comparers;
using ForecastingWorkingPopulation.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ForecastingWorkingPopulation.Database.Repositories
{
    public class PopulationRepository : IPopulationRepository
    {
        private readonly PopulationContext _dbContext;
        public PopulationRepository()
        {
            _dbContext = new PopulationContext();
            Init(_dbContext);
        }

        private void Init(PopulationContext dbContext)
        {
            //dbContext.Database.Migrate();
            FillRegions();
        }

        public List<RegionStatisticsDto> GetPopulationInRegion(int regionNumber)
        {
            return ConvertEntityToDto(_dbContext.PermanentPopulations.Where(entity => entity.RegionNumber == regionNumber)
                .Cast<BasePopulationEntity>()
                .ToList());
        }

        public List<RegionStatisticsDto> GetEconomyEmployedInRegion(int regionNumber)
        {
            return ConvertEntityToDto(_dbContext.EmployedEconomyPopulations.Where(entity => entity.RegionNumber == regionNumber)
                .Cast<BasePopulationEntity>()
                .ToList());
        }

        public List<RegionInfoEntity> GetAllRegions()
        {
            return _dbContext.Regions.ToList();
        }

        public List<BirthRateEntity>? GetBirthRateEntitiesByRegionNumber(int regionNumber)
        {
            return _dbContext.BirthRates.Where(entity => entity.RegionNumber == regionNumber)?.ToList();
        }

        public void SaveBirthRateEntyties(List<BirthRateEntity> dtos)
        {
            _dbContext.BirthRates.ExecuteDelete();
            _dbContext.BirthRates.AddRange(dtos);
            _dbContext.SaveChanges();
        }

        public void SaveEmployedEconomyEntyties(int regionNumber, List<RegionStatisticsDto> dtos)
        {
            var entities = GetEconomyEmployedInRegion(regionNumber);

            var updatedDtos = UpdateDtos(entities, dtos);
            var newDtos = dtos.Except(updatedDtos, new RegionStatisticComparer()).ToList();

            var updatedEntities = ConvertToEntity<EmployedEconomyPopulationInRegionEntity>(updatedDtos, regionNumber);
            var newEntities = ConvertToEntity<EmployedEconomyPopulationInRegionEntity>(newDtos, regionNumber);

            foreach (var entity in updatedEntities)
                UpdateEmployedEconomyEntity(entity.Id, entity.Size);

            _dbContext.EmployedEconomyPopulations.AddRange(newEntities);

            _dbContext.SaveChanges();
        }

        public void SavePermanentPopulationEntyties(int regionNumber, List<RegionStatisticsDto> dtos)
        {
            var entities = GetPopulationInRegion(regionNumber);

            var updatedDtos = UpdateDtos(entities, dtos);
            var newDtos = dtos.Except(updatedDtos, new RegionStatisticComparer()).ToList();

            var updatedEntities = ConvertToEntity<PermanentPopulationInRegionEntity>(updatedDtos, regionNumber);
            var newEntities = ConvertToEntity<PermanentPopulationInRegionEntity>(newDtos, regionNumber);

            foreach (var entity in updatedEntities)
                UpdatePermonentEntity(entity.Id, entity.Size);

            _dbContext.PermanentPopulations.AddRange(newEntities);

            _dbContext.SaveChanges();
        }

        private void UpdateEmployedEconomyEntity(int Id, int newValue)
        {
            var entity = _dbContext.EmployedEconomyPopulations.FirstOrDefault(enity => enity.Id == Id);
            if (entity is null)
                return;

            entity.Size = newValue;
        }

        private void UpdatePermonentEntity(int Id, int newValue)
        {
            var entity = _dbContext.PermanentPopulations.FirstOrDefault(enity => enity.Id == Id);
            if (entity is null)
                return;

            entity.Size = newValue;
        }

        private List<RegionStatisticsDto> UpdateDtos(List<RegionStatisticsDto> entities, List<RegionStatisticsDto> dtos)
        {
            var updatedEntities = new List<RegionStatisticsDto>();
            foreach (var dto in dtos)
            {
                var currentEntity = entities.FirstOrDefault(entity => entity.Age == dto.Age && entity.Gender == dto.Gender && entity.Year == dto.Year);
                if (currentEntity != null)
                {
                    currentEntity.SummaryByYear = dto.SummaryByYear;
                    updatedEntities.Add(currentEntity);
                }
            }

            return updatedEntities;
        }

        private List<RegionStatisticsDto> ConvertEntityToDto(List<BasePopulationEntity> entities)
        {
            return entities.Select(entity =>
            {
                return new RegionStatisticsDto
                {
                    Id = entity.Id,
                    Age = entity.Age,
                    Gender = entity.Gender,
                    SummaryByYear = entity.Size,
                    Year = entity.Year,
                };
            }).ToList();
        }

        private List<TEnity> ConvertToEntity<TEnity>(List<RegionStatisticsDto> dtos, int regionNumber) where TEnity : BasePopulationEntity, new()
        {
            return dtos.Select(dto =>
            {
                return new TEnity
                {
                    Id = dto.Id,
                    RegionNumber = regionNumber,
                    Age = dto.Age,
                    Gender = dto.Gender,
                    Size = dto.SummaryByYear,
                    Year = dto.Year,
                };
            }).ToList();
        }

        private void FillRegions()
        {
            if (_dbContext.Regions.Count() > 1)
                return;

            var regions = RegionRepository.GetRegions();
            _dbContext.AddRange(regions);
            _dbContext.SaveChanges();
        }

        public RegionCoefficientSettingsEntity? GetRegionCoefficientSettings(int regionNumber)
        {
            if (!_dbContext.RegionCoefficientSettings.Any())
                return null;

            return _dbContext.RegionCoefficientSettings
                .FirstOrDefault(settings => settings.RegionNumber == regionNumber);
        }

        public void SaveRegionCoefficientSettings(RegionCoefficientSettingsEntity settings)
        {
            var existingSettings = GetRegionCoefficientSettings(settings.RegionNumber);

            if (existingSettings != null)
            {
                // Обновляем существующие настройки
                existingSettings.Coefficient2019 = settings.Coefficient2019;
                existingSettings.Coefficient2020 = settings.Coefficient2020;
                existingSettings.Coefficient2021 = settings.Coefficient2021;
                existingSettings.Coefficient2022 = settings.Coefficient2022;
                existingSettings.Coefficient2023 = settings.Coefficient2023;
                existingSettings.Coefficient2024 = settings.Coefficient2024;
                existingSettings.MinAge = settings.MinAge;
                existingSettings.MaxAge = settings.MaxAge;
                existingSettings.CoefficientLimit = settings.CoefficientLimit;
                existingSettings.DisableCoefficientCutoff = settings.DisableCoefficientCutoff;
            }
            else
            {
                // Добавляем новые настройки
                _dbContext.RegionCoefficientSettings.Add(settings);
            }

            _dbContext.SaveChanges();
        }

        public RegionMainFormSettingsEntity? GetRegionMainFormSettings(int regionNumber)
        {
            if (!_dbContext.RegionMainFormSettings.Any())
                return null;

            return _dbContext.RegionMainFormSettings
                .FirstOrDefault(settings => settings.RegionNumber == regionNumber);
        }

        public void SaveRegionMainFormSettings(RegionMainFormSettingsEntity settings)
        {
            var existingSettings = GetRegionMainFormSettings(settings.RegionNumber);

            if (existingSettings != null)
            {
                // Обновляем существующие настройки
                existingSettings.SelectedGender = settings.SelectedGender;
                existingSettings.SelectedSmoothing = settings.SelectedSmoothing;
                existingSettings.WindowSize = settings.WindowSize;
            }
            else
            {
                // Добавляем новые настройки
                _dbContext.RegionMainFormSettings.Add(settings);
            }

            _dbContext.SaveChanges();
        }

        public RegionEconomyEmploedFormSettingsEntity? GetRegionEconomyEmploedFormSettings(int regionNumber)
        {
            if (!_dbContext.RegionEconomyEmploedFormSettings.Any())
                return null;

            return _dbContext.RegionEconomyEmploedFormSettings
                .FirstOrDefault(settings => settings.RegionNumber == regionNumber);
        }

        public void SaveRegionEconomyEmploedFormSettings(RegionEconomyEmploedFormSettingsEntity settings)
        {
            var existingSettings = GetRegionEconomyEmploedFormSettings(settings.RegionNumber);

            if (existingSettings != null)
            {
                // Обновляем существующие настройки
                existingSettings.SelectedGender = settings.SelectedGender;
                existingSettings.SelectedSmoothing = settings.SelectedSmoothing;
                existingSettings.WindowSize = settings.WindowSize;
                existingSettings.InEconomySelectedSmoothing = settings.InEconomySelectedSmoothing;
                existingSettings.InEconomyWindowSize = settings.InEconomyWindowSize;
            }
            else
            {
                // Добавляем новые настройки
                _dbContext.RegionEconomyEmploedFormSettings.Add(settings);
            }

            _dbContext.SaveChanges();
        }
    }
}
