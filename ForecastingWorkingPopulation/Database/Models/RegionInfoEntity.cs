using System.ComponentModel.DataAnnotations;

namespace ForecastingWorkingPopulation.Database.Models
{
    public class RegionInfoEntity
    {
        [Key]
        public int Number { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
