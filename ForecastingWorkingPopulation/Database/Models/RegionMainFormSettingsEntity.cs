using System.ComponentModel.DataAnnotations;
using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Database.Models
{
    /// <summary>
    /// Настройки главной формы для региона
    /// </summary>
    public class RegionMainFormSettingsEntity
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
        /// Выбранный тип сглаживания в комбобоксе
        /// </summary>
        public int SelectedSmoothing { get; set; }
        public int SelectedCoefficientSmoothing { get; set; }
        /// <summary>
        /// Размер окна сглаживания
        /// </summary>
        public int WindowSize { get; set; }
        public int CoefficientWindowSize { get; set; }

        public int SelectedCoefficientProcessing { get; set; }
        public int DeltaValue { get; set; }

        /// <summary>
        /// Максимальное значение по оси Y для графика постоянного населения
        /// </summary>
        public double PermanentPopulationMaximumY { get; set; }
    }
}
