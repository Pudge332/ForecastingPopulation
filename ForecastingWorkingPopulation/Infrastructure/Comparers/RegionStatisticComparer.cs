using ForecastingWorkingPopulation.Models.Dto;
using System.Diagnostics.CodeAnalysis;


namespace ForecastingWorkingPopulation.Infrastructure.Comparers
{
    public class RegionStatisticComparer : IEqualityComparer<RegionStatisticsDto>
    {
        public bool Equals(RegionStatisticsDto first, RegionStatisticsDto second)
        {
            if(first.Age == second.Age && first.Year == second.Year && first.Gender == second.Gender)
                return true;

            return false;
        }

        public int GetHashCode([DisallowNull] RegionStatisticsDto obj)
        {
            int hashObjCode = obj.Year.GetHashCode();
            int hashObjValue = obj.Age.GetHashCode();

            return hashObjValue ^ hashObjCode;
        }
    }
}
