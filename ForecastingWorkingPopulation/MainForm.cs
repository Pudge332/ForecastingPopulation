using ForecastingWorkingPopulation.Contracts.Interfaces;
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
        public MainForm(IPopulationRepository repository)
        {
            InitializeComponent();
            _populationRepository = repository;
            _linearGraphPainter = new LinearGraphPainter();
            InitForm();
        }

        private void InitForm()
        {
            PaintByGender(GenderComboBox.All);
            InitComboboxes();
        }

        private void InitComboboxes()
        {
            comboBox1.Items.Add("Все");
            comboBox1.Items.Add("Мужчины");
            comboBox1.Items.Add("Женщины");

            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += GenderComboBoxChanged;
        }

        private void InitEconomyEnployedGraphs(GenderComboBox comboValue)
        {
            
        }

        private void GenderComboBoxChanged(object sender, EventArgs e)
        {
            PaintByGender((GenderComboBox)comboBox1.SelectedIndex);
        }

        private void PaintByGender(GenderComboBox comboValue)
        {
            chart1.Series.Clear();
            var groupedByYear = _populationRepository.GetEconomyEmployedInRegion(10)
                .GroupBy(dto => dto.Year);

            foreach (var group in groupedByYear)
            {
                var currentGroup = SelectByGender(comboValue, group);
                var xValues = currentGroup.Select(dto => dto.Age).ToList();
                var yValues = currentGroup.Select(dto => dto.SummaryByYear).ToList();

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
    }
}
