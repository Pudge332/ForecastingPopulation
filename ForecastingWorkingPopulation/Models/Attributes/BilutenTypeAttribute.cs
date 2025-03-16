namespace ForecastingWorkingPopulation.Models.Attributes
{
    public class BilutenTypeAttribute : Attribute
    {
        /// <summary>
        /// Адрес ячейки с названием региона 
        /// </summary>
        public string RegionNameAddress { get; set; }
        /// <summary>
        /// Смещение столбца возраста
        /// </summary>
        public int AgeColumnOffset { get; set; }
        public BilutenTypeAttribute(string regionNameAddress, int ageColumnOffset)
        {
            RegionNameAddress = regionNameAddress;
            AgeColumnOffset = ageColumnOffset;
        }
    }
}
