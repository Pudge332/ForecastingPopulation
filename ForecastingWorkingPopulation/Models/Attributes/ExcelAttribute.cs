namespace ForecastingWorkingPopulation.Models.Attributes
{
    public sealed class ExcelAttribute : Attribute
    {
        /// <summary>
        /// Номер столбца
        /// </summary>
        public int ColumnNumber { get; set; }
        /// <summary>
        /// Столбец связанный с годом?
        /// </summary>
        public bool Year { get; set; }
        /// <summary>
        /// Тип данных
        /// </summary>
        public Type Type { get; set; }

        public ExcelAttribute(int columnNumber, bool year, Type type)
        {
            ColumnNumber = columnNumber;
            Year = year;
            Type = type;
        }
    }
}
