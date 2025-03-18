using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;
using static OfficeOpenXml.ExcelErrorValue;

namespace ForecastingWorkingPopulation
{
    public partial class LifeExpectancyCoefficientForm: Form
    {
        private readonly IPopulationRepository _populationRepository;
        private readonly LinearGraphPainter _painter;

        public LifeExpectancyCoefficientForm()
        {
            InitializeComponent();
            _populationRepository = new PopulationRepository();
            _painter = new LinearGraphPainter();
            CalculateAndPaintCoefficent();
        }

        private void CalculateAndPaintCoefficent()
        {
            var regions = RegionRepository.GetRegions();
            var countRegionsWithNotEmptyData = 0;
            var ages = new List<double>();
            var coefficentDtos = new List<RegionCoefficentDto>();
            var years = new List<int>() { 2019, 2024 };
            foreach (var year in years) {
                foreach (var region in regions)
                {
                    var dtos = _populationRepository.GetPopulationInRegion(region.Number);

                    foreach (var group in dtos.Where(dto => dto.Year == year))
                        coefficentDtos.AddRange(GetData(dtos.Where(dto => dto.Year == year)));
                }

                var grouppedDtos = coefficentDtos.GroupBy(dto => dto.Age);

                var xValues = new List<double>();
                var yValues = new List<double>();

                foreach (var group in grouppedDtos)
                {
                    xValues.Add(group.Key);
                    yValues.Add(group.Sum(item => item.Coefficent) / group.Count());
                }

                var series = _painter.PainLinearGraph(year.ToString(), xValues, yValues);
                chart1.Series.Add(series);
            }
        }

        private List<RegionCoefficentDto> GetData(IEnumerable<RegionStatisticsDto> dtos)
        {
            var coefficents = new List<RegionCoefficentDto>();
            var maxAge = 75;
            var minAge = 10;

            var ages = dtos.Where(dto => dto.Age < maxAge && dto.Age > minAge).Select(dto => dto.Age).Distinct().ToList();
            ages.Sort();

            foreach(var age in dtos.Where(dto => dto.Age < maxAge && dto.Age > minAge).GroupBy(dto => dto.Age))
            {
                var summaryCoefficent = 0.0;
                var dtosCount = 0;

                foreach (var group in age.ToList()) 
                {
                    var currentAges = dtos.FirstOrDefault(dto => dto.Age == age.Key + 1 && dto.Gender == group.Gender);
                    var coefficent = (double)currentAges.SummaryByYear / group.SummaryByYear;

                    if (coefficent > 1)
                        coefficent = 1;

                    dtosCount++;
                    summaryCoefficent += coefficent;
                }

                summaryCoefficent = summaryCoefficent / dtosCount;

                if (summaryCoefficent > 1)
                    summaryCoefficent = 1;
                coefficents.Add(new RegionCoefficentDto
                {
                    Age = age.Key,
                    Coefficent = summaryCoefficent
                });
            }

            return coefficents;
        }


    }
}
