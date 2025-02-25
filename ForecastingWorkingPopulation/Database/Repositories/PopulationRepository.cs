using ForecastingWorkingPopulation.Contracts.Interfaces;
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

        public void SaveEntityByRegion<TEntity>(int regionNumber, List<RegionStatisticsDto> dtos) where TEntity : BasePopulationEntity, new()
        {
            var entities = GetEconomyEmployedInRegion(regionNumber);

            var updatedDtos = UpdateDtos(entities, dtos);
            var newDtos = dtos.Except(updatedDtos, new RegionStatisticComparer()).ToList();

            var updatedEntities = ConvertToEntity<TEntity>(updatedDtos, regionNumber);
            var newEntities = ConvertToEntity<TEntity>(newDtos, regionNumber);

            foreach (var entity in updatedEntities)
                UpdateEmployedEconomyEntity(entity.Id, entity.Size);

            _dbContext.AddRange(newEntities);

            _dbContext.SaveChanges();
        }

        private void UpdateEmployedEconomyEntity(int Id, int newValue)
        {
            var entity = _dbContext.EmployedEconomyPopulations.FirstOrDefault(enity => enity.Id == Id);
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

            var regions = GetRegions();
            _dbContext.AddRange(regions);
            _dbContext.SaveChanges();
        }

        #region Хардкод регоинов 
        private List<RegionInfoEntity> GetRegions()
        {
            return new List<RegionInfoEntity>() { new RegionInfoEntity { Name = "Республика Адыгея (Адыгея)", Number = 1, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Башкортостан", Number = 2, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Бурятия", Number = 3, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Алтай", Number = 4, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Дагестан", Number = 5, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Ингушетия", Number = 6, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Кабардино-Балкарская Республика", Number = 7, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Калмыкия", Number = 8, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Карачаево-Черкесская Республика", Number = 9, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Карелия", Number = 10, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Коми", Number = 11, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Марий Эл", Number = 12, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Мордовия", Number = 13, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Саха (Якутия)", Number = 14, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Северная Осетия - Алания", Number = 15, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Татарстан (Татарстан)", Number = 16, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Тыва", Number = 17, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Удмуртская Республика", Number = 18, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Республика Хакасия", Number = 19, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Чеченская Республика", Number = 20, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Чувашская Республика - Чувашия", Number = 21, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Алтайский край", Number = 22, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Краснодарский край", Number = 23, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Красноярский край", Number = 24, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Приморский край", Number = 25, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ставропольский край", Number = 26, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Хабаровский край", Number = 27, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Амурская область", Number = 28, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Архангельская область", Number = 29, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Астраханская область", Number = 30, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Белгородская область", Number = 31, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Брянская область", Number = 32, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Владимирская область", Number = 33, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Волгоградская область", Number = 34, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Вологодская область", Number = 35, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Воронежская область", Number = 36, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ивановская область", Number = 37, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Иркутская область", Number = 38, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Калининградская область", Number = 39, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Калужская область", Number = 40, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Камчатский край", Number = 41, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Кемеровская область", Number = 42, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Кировская область", Number = 43, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Костромская область", Number = 44, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Курганская область", Number = 45, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Курская область", Number = 46, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ленинградская область", Number = 47, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Липецкая область", Number = 48, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Магаданская область", Number = 49, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Московская область", Number = 50, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Мурманская область", Number = 51, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Нижегородская область", Number = 52, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Новгородская область", Number = 53, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Новосибирская область", Number = 54, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Омская область", Number = 55, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Оренбургская область", Number = 56, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Орловская область", Number = 57, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Пензенская область", Number = 58, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Пермский край", Number = 59, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Псковская область", Number = 60, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ростовская область", Number = 61, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Рязанская область", Number = 62, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Самарская область", Number = 63, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Саратовская область", Number = 64, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Сахалинская область", Number = 65, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Свердловская область", Number = 66, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Смоленская область", Number = 67, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Тамбовская область", Number = 68, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Тверская область", Number = 69, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Томская область", Number = 70, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Тульская область", Number = 71, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Тюменская область", Number = 72, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ульяновская область", Number = 73, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Челябинская область", Number = 74, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Забайкальский край", Number = 75, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ярославская область", Number = 76, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "г. Москва", Number = 77, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Санкт-Петербург", Number = 78, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Еврейская автономная область", Number = 79, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ненецкий автономный округ", Number = 83, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ханты-Мансийский автономный округ - Югра", Number = 86, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Чукотский автономный округ", Number = 87, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Ямало-Ненецкий автономный округ", Number = 89, LastUpdateTime = DateTime.Now },
new RegionInfoEntity { Name = "Иные территории, включая город и космодром Байконур", Number = 99, LastUpdateTime = DateTime.Now } };
        }

        #endregion
    }
}
