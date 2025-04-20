using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure;
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
        private bool _isSetting = false;
        private int _windowSize = 5; // Значение по умолчанию для размера окна сглаживания

        public MainForm()
        {
            InitializeComponent();
            _populationRepository = new PopulationRepository();
            _linearGraphPainter = new LinearGraphPainter();
            InitForm();
        }

        private void InitForm()
        {
            InitComboboxes();
            LoadSettings();
            PaintByGender((GenderComboBox)comboBox1.SelectedIndex, (SmoothComboBox)comboBox2.SelectedIndex);
        }

        private void LoadSettings()
        {
            _isSetting = true;
            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            var settings = _populationRepository.GetRegionMainFormSettings(regionId);
            if (settings != null)
            {
                // Загружаем настройки
                comboBox1.SelectedIndex = settings.SelectedGender;
                comboBox2.SelectedIndex = settings.SelectedSmoothing;
                windowSizeNumericUpDown.Value = settings.WindowSize;
                _windowSize = settings.WindowSize;
            }
            _isSetting = false;
        }

        private void SaveSettings()
        {
            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            var settings = new ForecastingWorkingPopulation.Database.Models.RegionMainFormSettingsEntity
            {
                RegionNumber = regionId,
                SelectedGender = comboBox1.SelectedIndex,
                SelectedSmoothing = comboBox2.SelectedIndex,
                WindowSize = (int)windowSizeNumericUpDown.Value
            };

            _populationRepository.SaveRegionMainFormSettings(settings);
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

        private void btnLifeExpectancy_Click(object sender, EventArgs e)
        {
            // Открываем форму коэффициентов ожидаемой продолжительности жизни
            var lifeExpectancyForm = new LifeExpectancyCoefficientForm();
            lifeExpectancyForm.Show();
        }

        private void SmoothingComboBoxChanged(object sender, EventArgs e)
        {
            if (_isSetting)
                return;

            PaintByGender((GenderComboBox)comboBox1.SelectedIndex, (SmoothComboBox)comboBox2.SelectedIndex);
            SaveSettings();
        }

        private void GenderComboBoxChanged(object sender, EventArgs e)
        {
            if (_isSetting)
                return;

            PaintByGender((GenderComboBox)comboBox1.SelectedIndex, (SmoothComboBox)comboBox2.SelectedIndex);
            SaveSettings();
        }

        private void WindowSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_isSetting)
                return;

            _windowSize = (int)windowSizeNumericUpDown.Value;
            PaintByGender((GenderComboBox)comboBox1.SelectedIndex, (SmoothComboBox)comboBox2.SelectedIndex);
            SaveSettings();
        }

        private void PaintByGender(GenderComboBox genderComboValue, SmoothComboBox smoothComboValue)
        {
            chart1.Series.Clear();
            chart2.Series.Clear();

            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            var economyEmploedDtos = _populationRepository.GetEconomyEmployedInRegion(regionId);
            var populationDtos = _populationRepository.GetPopulationInRegion(regionId);

            // Обновляем заголовок формы, чтобы показать текущий регион
            this.Text = $"Анализ данных региона (ID: {regionId})";

            PaintChartData(economyEmploedDtos, genderComboValue, smoothComboValue, chart1);
            PaintChartData(populationDtos, genderComboValue, smoothComboValue, chart2);
        }

        private void PaintChartData(List<RegionStatisticsDto> dtos, GenderComboBox genderComboValue, SmoothComboBox smoothComboValue, Chart currentChart)
        {
            var groupedByYear = dtos
                .GroupBy(dto => dto.Year);

            foreach (var group in groupedByYear)
            {
                var currentGroup = SelectByGender(genderComboValue, group);
                var xValues = currentGroup.Select(dto => dto.Age).Select(Convert.ToDouble).ToList();
                var yValues = currentGroup.Select(dto => dto.SummaryByYear).Select(Convert.ToDouble).ToList();
                if ((int)smoothComboValue > 0)
                    yValues = MovingAverageSmoothing(currentGroup, windowSize: _windowSize, (int)smoothComboValue);

                var series = _linearGraphPainter.PainLinearGraph($"Год {group.Key}", xValues, yValues);
                currentChart.Series.Add(series);
                CalculationStorage.Instance.StoreEconomyEmploedRegionStatistics(group.Key, currentGroup.ToList());
            }

            currentChart.ChartAreas[0].AxisX.Title = "Возраст";
            currentChart.ChartAreas[0].AxisY.Title = "Численность населения в тысячах";
        }

        private IEnumerable<RegionStatisticsDto> SelectByGender(GenderComboBox comboValue, IEnumerable<RegionStatisticsDto> dtos)
        {
            switch(comboValue)
            {
                case GenderComboBox.All:
                    return GetAll(dtos.ToList());

                case GenderComboBox.Males:
                    return dtos.Where(dto => dto.Gender == Gender.Male);

                case GenderComboBox.Females:
                    return dtos.Where(dto => dto.Gender == Gender.Female);
            }

            return Enumerable.Empty<RegionStatisticsDto>();
        }

        private IEnumerable<RegionStatisticsDto> GetAll(List<RegionStatisticsDto> dtos)
        {
            var summaryByGenderDtos = new List<RegionStatisticsDto>();
            var ages = dtos.Select(dto => dto.Age).Distinct();
            foreach(var age in ages)
            {
                var dtosByAge = dtos.Where(dto => dto.Age == age);
                summaryByGenderDtos.Add(new RegionStatisticsDto
                {
                    Age = age,
                    Year = dtosByAge.First().Year,
                    SummaryByYear = dtosByAge
                        .DefaultIfEmpty()
                        .Sum(dto => dto?.SummaryByYear) ?? 0
                });
            }

            return summaryByGenderDtos.Cast<RegionStatisticsDto>();
        }

        private List<double> MovingAverageSmoothing(IEnumerable<RegionStatisticsDto> data, int windowSize, int smoothingCount = 1)
        {
            var result = new List<RegionStatisticsDto>(data);
            foreach (var item in data)
                item.SummaryByYearSmoothed = item.SummaryByYear;

            for (int i = 0; i < smoothingCount; i++)
                result = MovingAverageSmoothing(result, windowSize);

            return result.Select(dto => dto.SummaryByYearSmoothed).ToList();
        }

        private List<RegionStatisticsDto> MovingAverageSmoothing(List<RegionStatisticsDto> data, int windowSize)
        {
            for (int i = 0; i < data.Count - windowSize; i++)
            {
                var smoothingValue = GetSumInRange(data, i, i + windowSize) / windowSize;
                data[i].SummaryByYearSmoothed = smoothingValue;
            }

            return data;
        }

        private double GetSumInRange(List<RegionStatisticsDto> data, int startIndex, int endIndex)
        {
            var result = 0.0;
            for (int i = startIndex; i < endIndex; i++)
                result += data[i].SummaryByYearSmoothed;

            return result;
        }
    }
}
