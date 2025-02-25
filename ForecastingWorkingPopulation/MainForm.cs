using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;
using System.Windows.Forms.DataVisualization.Charting;

namespace ForecastingWorkingPopulation
{
    public partial class MainForm : Form
    {
        private readonly IPopulationRepository _populationRepository;
        private readonly LinearGraphPainter _linearGraphPainter;
        public MainForm()
        {
            InitializeComponent();
            _populationRepository = new PopulationRepository(); 
            _linearGraphPainter = new LinearGraphPainter();
            InitForm();
        }

        private void InitForm()
        {
            PaintByGender(GenderComboBox.All, SmoothComboBox.NO);
            InitComboboxes();
        }

        private void InitComboboxes()
        {
            comboBox1.Items.Add("Все");
            comboBox1.Items.Add("Мужчины");
            comboBox1.Items.Add("Женщины");
            comboBox2.Items.Add("NO");
            comboBox2.Items.Add("1X");
            comboBox2.Items.Add("2X");
            comboBox2.Items.Add("3X");

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += GenderComboBoxChanged;
            comboBox2.SelectedIndexChanged += SmoothingComboBoxChanged;
        }

        private void InitEconomyEnployedGraphs(GenderComboBox comboValue)
        {
            
        }

        private void SmoothingComboBoxChanged(object sender, EventArgs e)
        {
            PaintByGender((GenderComboBox)comboBox1.SelectedIndex, (SmoothComboBox)comboBox2.SelectedIndex);
        }

        private void GenderComboBoxChanged(object sender, EventArgs e)
        {
            PaintByGender((GenderComboBox)comboBox1.SelectedIndex, (SmoothComboBox)comboBox2.SelectedIndex);
        }

        private void PaintByGender(GenderComboBox genderComboValue, SmoothComboBox smoothComboValue)
        {
            chart1.Series.Clear();
            var groupedByYear = _populationRepository.GetEconomyEmployedInRegion(10)
                .GroupBy(dto => dto.Year);

            foreach (var group in groupedByYear)
            {
                var currentGroup = SelectByGender(genderComboValue, group);
                var xValues = currentGroup.Select(dto => dto.Age).Select(Convert.ToDouble).ToList();
                var yValues = currentGroup.Select(dto => dto.SummaryByYear).Select(Convert.ToDouble).ToList();
                if ((int)smoothComboValue > 0)
                    yValues = MovingAverageSmoothing(yValues, windowSize: 5, (int)smoothComboValue);

                var series = _linearGraphPainter.PainLinearGraph($"Год {group.Key}", xValues, yValues);
                chart1.Series.Add(series);
            }

            chart1.ChartAreas[0].AxisX.Title = "Возраст";
            chart1.ChartAreas[0].AxisY.Title = "Численность населения в тысячах";
        }

        private IEnumerable<RegionStatisticsDto> SelectByGender(GenderComboBox comboValue, IEnumerable<RegionStatisticsDto> dtos)
        {
            switch(comboValue)
            {
                case GenderComboBox.All:
                    return dtos;

                case GenderComboBox.Males:
                    return dtos.Where(dto => dto.Gender == Gender.Male);

                case GenderComboBox.Females:
                    return dtos.Where(dto => dto.Gender == Gender.Female);
            }

            return Enumerable.Empty<RegionStatisticsDto>();
        }

        private List<double> MovingAverageSmoothing(List<double> data, int windowSize, int smoothingCount = 1)
        {
            var result = new List<double>(data);

            for (int i = 0; i < smoothingCount; i++)
                result = MovingAverageSmoothing(result, windowSize);

            return result;
        }

        private List<double> MovingAverageSmoothing(List<double> data, int windowSize)
        {
            var result = new List<double>();

            for (int i = 0; i < data.Count - windowSize; i++)
                result.Add(GetSumInRange(data, i, i + windowSize) / windowSize);

            return result;
        }

        private double GetSumInRange(List<double> data, int startIndex, int endIndex)
        {
            var result = 0.0;
            for (int i = startIndex; i < endIndex; i++)
                result += data[i];

            return result;
        }
    }
}
