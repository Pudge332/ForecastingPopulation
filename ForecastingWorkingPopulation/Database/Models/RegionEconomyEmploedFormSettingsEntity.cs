using System.ComponentModel.DataAnnotations;
using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation.Database.Models
{
    /// <summary>
    /// Настройки формы занятых в экономике для региона
    /// </summary>
    public class RegionEconomyEmploedFormSettingsEntity
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

        /// <summary>
        /// Размер окна сглаживания
        /// </summary>
        public int WindowSize { get; set; }

        /// <summary>
        /// Выбранный тип сглаживания для графика уровня занятости
        /// </summary>
        public int InEconomySelectedSmoothing { get; set; }

        /// <summary>
        /// Размер окна сглаживания для графика уровня занятости
        /// </summary>
        public int InEconomyWindowSize { get; set; }

        /// <summary>
        /// Максимальное значение по оси Y для графика занятых в экономике
        /// </summary>
        public double EconomyEmploedMaxY { get; set; }

        /// <summary>
        /// Максимальное значение по оси Y для графика занятых в экономике со сглаживанием
        /// </summary>
        public double EconomyEmploedSmoothMaxY { get; set; }

        /// <summary>
        /// Максимальное значение по оси Y для графика уровня занятости в экономике
        /// </summary>
        public double InEconomyLevelMaxY { get; set; }

        /// <summary>
        /// Максимальное значение по оси Y для графика уровня занятости в экономике со сглаживанием
        /// </summary>
        public double InEconomyLevelSmoothMaxY { get; set; }
    }
}
