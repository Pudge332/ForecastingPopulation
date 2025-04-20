using ForecastingWorkingPopulation.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ForecastingWorkingPopulation.Database.Context
{
    public class PopulationContext : DbContext
    {
        public DbSet<EmployedEconomyPopulationInRegionEntity> EmployedEconomyPopulations { get; set; }
        public DbSet<PermanentPopulationInRegionEntity> PermanentPopulations { get; set; }
        public DbSet<RegionInfoEntity> Regions { get; set; }
        public DbSet<RegionCoefficientSettingsEntity> RegionCoefficientSettings { get; set; }
        public DbSet<RegionMainFormSettingsEntity> RegionMainFormSettings { get; set; }
        public DbSet<RegionEconomyEmploedFormSettingsEntity> RegionEconomyEmploedFormSettings { get; set; }
        public DbSet<BirthRateEntity> BirthRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Установка значений по умолчанию для новых полей
            modelBuilder.Entity<RegionEconomyEmploedFormSettingsEntity>()
                .Property(e => e.EconomyEmploedMaxY)
                .HasDefaultValue(0);

            modelBuilder.Entity<RegionEconomyEmploedFormSettingsEntity>()
                .Property(e => e.EconomyEmploedSmoothMaxY)
                .HasDefaultValue(0);

            modelBuilder.Entity<RegionEconomyEmploedFormSettingsEntity>()
                .Property(e => e.InEconomyLevelMaxY)
                .HasDefaultValue(0);

            modelBuilder.Entity<RegionEconomyEmploedFormSettingsEntity>()
                .Property(e => e.InEconomyLevelSmoothMaxY)
                .HasDefaultValue(0);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Population.db");
        }
    }
}
