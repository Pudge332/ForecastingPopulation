using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;

namespace ForecastingWorkingPopulation
{
    public partial class LifeExpectancyCoefficientForm : Form
    {
        private readonly IPopulationRepository _populationRepository;
        private readonly LinearGraphPainter _painter;
        private int MinAge = 0;
        private int MaxAge = 80;

        public LifeExpectancyCoefficientForm()
        {
            InitializeComponent();
            _populationRepository = new PopulationRepository();
            _painter = new LinearGraphPainter();
            Init();
            CalculateAndPaintCoefficent(10);
        }

        private void Init()
        {
            label1.Text = "Отключить обрезку коэффицентов > 1";
            label2.Text = "100% + *% = ";
            label3.Text = "Минимальный возраст";
            label4.Text = "Максимальный возраст";
            numericUpDown2.Value = MinAge;
            numericUpDown3.Value = MaxAge;
        }

        private void CalculateAndPaintCoefficent(int regionNumber)
        {
            chart1.Series.Clear();
            var chartArea = chart1.ChartAreas[0];
            chartArea.AxisY.Maximum = 1.5;
            var regions = RegionRepository.GetRegions();
            var countRegionsWithNotEmptyData = 0;
            var ages = new List<double>();
            var coefficentDtos = new List<RegionCoefficentDto>();
            var dtos = _populationRepository.GetPopulationInRegion(regionNumber);
            var years = dtos.Select(dto => dto.Year).Distinct().OrderBy(x => x);
            foreach (var year in years)
            {
                if (year == years.Last())
                    continue;

                coefficentDtos.AddRange(GetData(dtos.Where(dto => dto.Year == year), dtos.Where(dto => dto.Year == year + 1)));
            }

            var grouppedDtos = GetAverage(coefficentDtos);

            var xValues = new List<double>();
            var yValues = new List<double>();

            foreach (var group in grouppedDtos)
            {
                xValues.Add(group.Age);
                yValues.Add(group.Coefficent);
            }

            var series = _painter.PainLinearGraph("КПЖ", xValues, yValues);
            chart1.Series.Add(series);

        }

        private double GetMaxCoefficentValue()
        {
            if (checkBox1.Checked)
                return double.PositiveInfinity;

            return 1 + Convert.ToDouble(numericUpDown1.Value / 100);
        }

        private List<RegionCoefficentDto> GetAverage(List<RegionCoefficentDto> dtos)
        {
            var coefficents = new List<RegionCoefficentDto>();
            var maxCoefficent = GetMaxCoefficentValue();

            foreach (var group in dtos.GroupBy(dto => dto.Age))
            {
                var byAge = group.ToList();
                var coefficent = byAge.Sum(dto => dto.Coefficent) / byAge.Count();
                if (coefficent > maxCoefficent)
                    coefficent = maxCoefficent;

                coefficents.Add(new RegionCoefficentDto
                {
                    Age = group.Key,
                    Coefficent = coefficent
                });
            }

            return coefficents;
        }

        private List<RegionCoefficentDto> GetData(IEnumerable<RegionStatisticsDto> currentYearDtos, IEnumerable<RegionStatisticsDto> nextYearDtos)
        {
            var coefficents = new List<RegionCoefficentDto>();
            var maxCoefficent = GetMaxCoefficentValue();

            foreach (var currentYearDto in currentYearDtos)
            {
                var nextYearDto = nextYearDtos.FirstOrDefault(dto => dto.Age == currentYearDto.Age + 1 && dto.Gender == currentYearDto.Gender);
                if (currentYearDto.SummaryByYear < 1 || nextYearDto == null)
                    continue;

                var coefficent = (double)nextYearDto.SummaryByYear / currentYearDto.SummaryByYear;

                if (coefficent > maxCoefficent)
                    coefficent = maxCoefficent;

                coefficents.Add(new RegionCoefficentDto
                {
                    Age = currentYearDto.Age,
                    Coefficent = coefficent
                });
            }

            return coefficents.Where(coefficent => coefficent.Age >= MinAge && coefficent.Age <= MaxAge).ToList();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MinAge = (int)numericUpDown2.Value;
            MaxAge = (int)numericUpDown3.Value;

            CalculateAndPaintCoefficent(10);
        }
    }
}
