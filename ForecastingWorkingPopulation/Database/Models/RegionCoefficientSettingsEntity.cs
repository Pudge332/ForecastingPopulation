using System.ComponentModel.DataAnnotations;

namespace ForecastingWorkingPopulation.Database.Models
{
    public class RegionCoefficientSettingsEntity
    {
        /// <summary>
        /// Номер региона (первичный ключ)
        /// </summary>
        [Key]
        public int RegionNumber { get; set; }

        /// <summary>
        /// Коэффициент для 2019 года
        /// </summary>
        public double Coefficient2019 { get; set; }

        /// <summary>
        /// Коэффициент для 2020 года
        /// </summary>
        public double Coefficient2020 { get; set; }

        /// <summary>
        /// Коэффициент для 2021 года
        /// </summary>
        public double Coefficient2021 { get; set; }

        /// <summary>
        /// Коэффициент для 2022 года
        /// </summary>
        public double Coefficient2022 { get; set; }

        /// <summary>
        /// Коэффициент для 2023 года
        /// </summary>
        public double Coefficient2023 { get; set; }

        /// <summary>
        /// Коэффициент для 2024 года
        /// </summary>
        public double Coefficient2024 { get; set; }

        /// <summary>
        /// Минимальный возраст
        /// </summary>
        public int MinAge { get; set; }

        /// <summary>
        /// Максимальный возраст
        /// </summary>
        public int MaxAge { get; set; }

        /// <summary>
        /// Значение для ограничения коэффициентов (numericUpDown1)
        /// </summary>
        public decimal CoefficientLimit { get; set; }

        /// <summary>
        /// Флаг отключения обрезки коэффициентов (checkBox1)
        /// </summary>
        public bool DisableCoefficientCutoff { get; set; }
    }
}
