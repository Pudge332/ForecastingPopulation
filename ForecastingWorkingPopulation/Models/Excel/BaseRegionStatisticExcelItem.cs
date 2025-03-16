using System.ComponentModel;
using System.Reflection;

namespace ForecastingWorkingPopulation.Models.Excel
{
    public class BaseRegionStatisticExcelItem
    {
        /// <summary>
        /// Количество из таблицы
        /// </summary>
        [Description("Здесь будет нужный год")]
        public double OrginalCount { get; set; }
        /// <summary>
        /// Интересующий нас год
        /// </summary>
        public int TargetYear { get; set; }
        /// <summary>
        /// Номер строки
        /// </summary>
        public int RowNumber { get; set; }

        public void SetDescription()
        {
            var descriptor = TypeDescriptor.GetProperties(this)["OrginalCount"];

            if (descriptor != null)
            {
                var attribute = descriptor.Attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
                var field = attribute?.GetType().GetField("description", BindingFlags.NonPublic | BindingFlags.Instance);
                field?.SetValue(attribute, TargetYear.ToString());
            }
        }
    }
}
