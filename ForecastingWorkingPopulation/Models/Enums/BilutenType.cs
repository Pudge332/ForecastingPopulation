using ForecastingWorkingPopulation.Models.Attributes;

namespace ForecastingWorkingPopulation.Models.Enums
{
    public enum BilutenType
    {
        /// <summary>
        /// Старый формат бюллетени
        /// </summary>
        [BilutenType("B4", 1)]
        Old,
        [BilutenType("A1", 0)]
        /// <summary>
        /// Новый формат бюллетени
        /// </summary>
        New
    }
}
