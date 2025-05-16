using System.ComponentModel.DataAnnotations;
using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Database.Models
{
    /// <summary>
    /// Настройки формы прогнозирования для региона
    /// </summary>
    public class RegionForecastionFormSettingsEntity
    {
        /// <summary>
        /// Номер региона (первичный ключ)
        /// </summary>
        [Key]
        public int RegionNumber { get; set; }

        /// <summary>
        /// Выбранный пол в комбобоксе
        /// </summary>
        public int SelectedGender { get; set; }

        /// <summary>
        /// Шаг прогнозирования (значение numericUpDown1)
        /// </summary>
        public int ForecastStep { get; set; }

        /// <summary>
        /// Конечный год прогнозирования (значение numericUpDown2)
        /// </summary>
        public int ForecastEndYear { get; set; }

        /// <summary>
        /// Максимальное значение по оси Y для графика прогноза
        /// </summary>
        public double ForecastMaxY { get; set; }
    }
}