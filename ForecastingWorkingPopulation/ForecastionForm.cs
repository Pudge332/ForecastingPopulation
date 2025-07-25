﻿using System.Data;
using System.Windows.Forms;
using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure;
using ForecastingWorkingPopulation.Infrastructure.Excel;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation
{
    public partial class ForecastionForm : Form
    {
        private List<double> xValues = new List<double>();
        private List<double> yValues = new List<double>();
        private double maxY = 0.0;
        private readonly IPopulationRepository _populationRepository;
        private readonly IExcelParser _excelParser;
        private readonly LinearGraphPainter _painter;
        private Dictionary<int, List<RegionStatisticsDto>> forecastDictionary;
        private bool _isSetting = false;

        public ForecastionForm()
        {
            _populationRepository = new PopulationRepository();
            _painter = new LinearGraphPainter();
            _excelParser = new ExcelParser();
            InitializeComponent();
            Init();
            forecastDictionary = new Dictionary<int, List<RegionStatisticsDto>>();
            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            // Обновляем заголовок формы, чтобы показать текущий регион
            this.Text = $"Прогноз занятого в экономике населения региона (ID: {regionId} - {RegionRepository.GetRegionNameById(regionId)})";

            // Загружаем настройки формы
            LoadSettings();

            // Создаем прогноз численности занятого в экономике населения по годам
            CreateEmployedInEconomyForecast(regionId);
        }

        private void Init()
        {
            comboBox1.Items.Add("Все");
            comboBox1.Items.Add("Мужчины");
            comboBox1.Items.Add("Женщины");
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
        }

        private void LoadSettings()
        {
            _isSetting = true;
            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            var settings = _populationRepository.GetRegionForecastionFormSettings(regionId);
            if (settings != null)
            {
                // Загружаем настройки
                comboBox1.SelectedIndex = settings.SelectedGender;
                numericUpDown1.Value = settings.ForecastStep;
                numericUpDown2.Value = settings.ForecastEndYear;

                // Устанавливаем максимальное значение для оси Y графика, если оно было сохранено
                if (settings.ForecastMaxY > 0 && forecast.ChartAreas.Count > 0)
                    forecast.ChartAreas[0].AxisY.Maximum = settings.ForecastMaxY;
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

            var settings = new ForecastingWorkingPopulation.Database.Models.RegionForecastionFormSettingsEntity
            {
                RegionNumber = regionId,
                SelectedGender = comboBox1.SelectedIndex,
                ForecastStep = (int)numericUpDown1.Value,
                ForecastEndYear = (int)numericUpDown2.Value
            };

            // Сохраняем текущее максимальное значение оси Y, если оно установлено
            if (forecast.ChartAreas.Count > 0 && forecast.ChartAreas[0].AxisY.Maximum != double.NaN)
            {
                settings.ForecastMaxY = forecast.ChartAreas[0].AxisY.Maximum;
            }

            _populationRepository.SaveRegionForecastionFormSettings(settings);
        }

        /// <summary>
        /// Создает прогноз численности занятого в экономике населения по годам
        /// </summary>
        /// <param name="regionId">Идентификатор региона</param>
        private void CreateEmployedInEconomyForecast(int regionId)
        {
            forecast.Series.Clear();
            forecast.ChartAreas[0].AxisX.Title = "Возраст";
            forecast.ChartAreas[0].AxisY.Title = "Численность занятого населения в тысячах";

            var permanentPopulation = CalculationStorage.Instance.PermanentPopulationForecast;
            var inEconomyLevelDtos = CalculationStorage.Instance.GetInEconomyLevelForecastData();

            CalculateForecast(permanentPopulation, inEconomyLevelDtos);
        }

        private void CalculateForecast(Dictionary<int, List<RegionStatisticsDto>> permanentPopulation, List<RegionInEconomyLevelDto> inEconomyLevelDtos)
        {
            forecastDictionary = new Dictionary<int, List<RegionStatisticsDto>>();
            var firstYear = permanentPopulation.First().Key;
            var xValues = new List<double>();
            var yValues = new List<double>();
            var step = (int)numericUpDown1.Value;
            maxY = 0.0;
            for (int year = firstYear; year < (int)numericUpDown2.Value; year += step)
                PaintForecast(permanentPopulation, inEconomyLevelDtos, year);

            PaintForecast(permanentPopulation, inEconomyLevelDtos, (int)numericUpDown2.Value);
        }

        private void PaintForecast(Dictionary<int, List<RegionStatisticsDto>> permanentPopulation, List<RegionInEconomyLevelDto> inEconomyLevelDtos, int year)
        {
            var forecastValues = new List<RegionStatisticsDto>();
            var lastYear = permanentPopulation[year];
            foreach (var age in lastYear.Select(dto => dto.Age).Distinct())
            {
                var dtosByYear = lastYear.Where(dto => dto.Age == age);
                foreach (var dto in dtosByYear)
                {
                    var inEconomyLevel = inEconomyLevelDtos.FirstOrDefault(x => x.Gender == dto.Gender && x.Age == dto.Age)?.Level;
                    if (inEconomyLevel == null)
                        inEconomyLevel = 0;

                    forecastValues.Add(new RegionStatisticsDto
                    {
                        SummaryByYearSmoothed = dto.SummaryByYearSmoothed * inEconomyLevel.Value,
                        Year = dto.Year,
                        Gender = dto.Gender,
                        Age = dto.Age
                    });
                }
            }
            (xValues, yValues) = SelectByGender(forecastValues);
            if (yValues.Any())
            {
                maxY = Math.Max(yValues.Max(), maxY);
                forecast.ChartAreas[0].AxisY.Maximum = maxY * 1.3;
            }
            forecast.ChartAreas[0].AxisX.Maximum = 70;
            forecast.ChartAreas[0].AxisX.Minimum = 12;
            forecast.Series.Add(_painter.PaintLinearGraph($"Прогноз на {year}", xValues, yValues));
            forecastDictionary.Add(year, forecastValues);
            FillForecastInOneAgeChart();
        }

        /// <summary>
        /// Заполняет график прогноза численности населения по возрастным группам
        /// </summary>
        /// <param name="regionId">Идентификатор региона</param>
        private void FillForecastInOneAgeChart()
        {
            forecastInOneAge.Series.Clear();
            forecastInOneAge.ChartAreas[0].AxisX.Title = "Год";
            forecastInOneAge.ChartAreas[0].AxisY.Title = "Численность населения в тысячах";
            var ageGroupps = new Dictionary<int, List<double>>();
            var years = new List<double>();
            for (int year = 2024; year < 2045; year++)
                years.Add(year);

            var forecastValues = forecastDictionary;
            foreach (var currentValues in forecastValues.Values)
            {
                for (int age = 20; age < 80; age += 10)
                {
                    var valuesInDictionary = new List<double>();
                    var currentYearGroupp = currentValues.Where(dto => dto.Age == age).ToList();
                    if (ageGroupps.TryGetValue(age, out valuesInDictionary))
                        valuesInDictionary.Add(currentYearGroupp.Sum(x => x.SummaryByYearSmoothed));
                    else
                        ageGroupps.Add(age, new List<double>() { currentYearGroupp.Sum(x => x.SummaryByYearSmoothed) });
                }
            }
            var seriesDataList = new List<ChartDataService.SeriesData>();
            foreach (var values in ageGroupps)
            {
                var series = _painter.PaintLinearGraph($"Возраст: {values.Key}", years, values.Value);
                forecastInOneAge.Series.Add(series);
                seriesDataList.Add(new ChartDataService.SeriesData
                {
                    SeriesName = "Возраст: {values.Key}",
                    YValues = values.Value,
                    XValues = years
                });
            }
        }

        private (List<double>, List<double>) SelectByGender(List<RegionStatisticsDto> dtos)
        {
            switch ((GenderComboBox)comboBox1.SelectedIndex)
            {
                case GenderComboBox.All:
                    return GetValuesForChart(dtos);
                    break;
                case GenderComboBox.Males:
                    return GetValuesForChart(dtos, Gender.Male);
                    break;
                default:
                    return GetValuesForChart(dtos, Gender.Female);
            }
        }

        private (List<double>, List<double>) GetValuesForChart(List<RegionStatisticsDto> dtos, Gender? gender = null)
        {
            var xValues = new List<double>();
            var yValues = new List<double>();

            foreach (var age in dtos.Select(dto => dto.Age).Distinct())
            {
                xValues.Add(age);
                var currentDtos = dtos.Where(dto => dto.Age == age);
                if (gender != null)
                    currentDtos = currentDtos.Where(dto => dto.Gender == gender);

                yValues.Add(currentDtos.Sum(dto => dto.SummaryByYearSmoothed));
            }

            return (xValues, yValues);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            SaveSettings();
            FormRouting.PreviousForm(3, this);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            CreateEmployedInEconomyForecast(CalculationStorage.Instance.CurrentRegion);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            CreateEmployedInEconomyForecast(CalculationStorage.Instance.CurrentRegion);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            CreateEmployedInEconomyForecast(CalculationStorage.Instance.CurrentRegion);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Выберите директорию для сохранения прогноза в формате Excel";
            var regionName = RegionRepository.GetRegionNameById(CalculationStorage.Instance.CurrentRegion);

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _excelParser.FillForecastFile(folderBrowserDialog1.SelectedPath, regionName, $"Прогноз численности занятого в экономике населения({regionName})", 
                    forecastDictionary, retrospectiveEconomyData: CalculationStorage.Instance.GetEconomyEmploedForecastData());
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки перед переходом на следующую форму
            SaveSettings();

            FormRouting.NextForm(3, this);
        }
    }
}
