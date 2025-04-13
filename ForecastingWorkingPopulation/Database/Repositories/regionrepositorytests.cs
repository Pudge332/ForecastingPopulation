using Xunit;
using ForecastingWorkingPopulation.Database.Models;
using System;
using System.Linq;

namespace ForecastingWorkingPopulation.Database.Repositories
{
    public class RegionRepositoryTests
    {
        [Fact]
        public void GetRegions_Returns82Regions()
        {
            // Act
            var result = RegionRepository.GetRegions();

            // Assert
            Assert.Equal(82, result.Count);
        }

        [Fact]
        public void GetRegions_NumbersAreUnique()
        {
            // Act
            var regions = RegionRepository.GetRegions();
            var numbers = regions.Select(r => r.Number).ToList();

            // Assert
            Assert.Equal(numbers.Distinct().Count(), numbers.Count);
        }

        [Fact]
        public void GetRegions_ContainsKeyRegions()
        {
            // Act
            var regions = RegionRepository.GetRegions();

            // Assert
            Assert.Contains(regions, r => r.Name == "Республика Адыгея (Адыгея)" && r.Number == 1);
            Assert.Contains(regions, r => r.Name == "г. Москва" && r.Number == 77);
            Assert.Contains(regions, r => r.Name == "Санкт-Петербург" && r.Number == 78);
        }

        [Fact]
        public void GetRegions_LastUpdateTimeIsRecent()
        {
            // Act
            var regions = RegionRepository.GetRegions();
            var minDate = DateTime.Now.AddMinutes(-5);

            // Assert
            Assert.All(regions, r => Assert.True(r.LastUpdateTime > minDate));
        }
    }
}
