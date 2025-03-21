﻿using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Context;
using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Infrastructure.Comparers;
using ForecastingWorkingPopulation.Models.Dto;

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
            //dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
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
    }
}
