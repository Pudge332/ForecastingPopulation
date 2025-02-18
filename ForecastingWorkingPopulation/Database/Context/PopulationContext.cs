using ForecastingWorkingPopulation.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ForecastingWorkingPopulation.Database.Context
{
    public class PopulationContext : DbContext
    {
        public DbSet<EmployedEconomyPopulationInRegionEntity> EmployedEconomyPopulations { get; set; }
        public DbSet<PermanentPopulationInRegionEntity> PermanentPopulations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Population.db");
        }
    }
}
