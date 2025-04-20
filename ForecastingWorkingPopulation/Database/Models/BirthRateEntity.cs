using System.ComponentModel.DataAnnotations;

namespace ForecastingWorkingPopulation.Database.Models
{
    public class BirthRateEntity
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; }
        public int RegionNumber { get; set; }
        public int BirthRate { get; set; }
    }
}
