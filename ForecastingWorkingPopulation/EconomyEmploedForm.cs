using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;
using System.Windows.Forms.DataVisualization.Charting;

namespace ForecastingWorkingPopulation
{
    public partial class EconomyEmploedForm : Form
    {
        private LinearGraphPainter _linearGraphPainter;
        private ChartDataService _chartService;
        private PopulationRepository _repository;
        private readonly SmoothingCalculator _smoothingCalculator;
        private bool _isSetting = false;
        private int _windowSize = 5; // Значение по умолчанию для размера окна сглаживания
        private int _inEconomyWindowSize = 5; // Значение по умолчанию для размера окна сглаживания графика уровня занятости
        private int _minAge = 12;
        private int _maxAge = 100;

        public EconomyEmploedForm()
        {
            _linearGraphPainter = new LinearGraphPainter();
            _chartService = new ChartDataService();
            _repository = new PopulationRepository();
            _smoothingCalculator = new SmoothingCalculator();

            InitializeComponent();
            InitControls();
            LoadSettings();

            var regionId = CalculationStorage.Instance.CurrentRegion;
            var data = _repository.GetEconomyEmployedInRegion(regionId);

            // Данные без сглаживания
            var economyEmploedData = _chartService.PrepareChartData(data, GenderComboBox.All, 5, 0, useSmoothing: false);
            PaintChart(economyEmploedData);

            // Данные со сглаживанием
            UpdateSmoothChart();

            // Данные для графика уровня занятости в экономике
            UpdateInEconomyLevelChart();
        }

        private void InitControls()
        {
            // Инициализация комбобоксов
            genderComboBox.Items.Add("Все");
            genderComboBox.Items.Add("Мужчины");
            genderComboBox.Items.Add("Женщины");

            smoothingComboBox.Items.Add("NO");
            smoothingComboBox.Items.Add("1X");
            smoothingComboBox.Items.Add("2X");
            smoothingComboBox.Items.Add("3X");

            // Заполняем комбобокс сглаживания для графика уровня занятости
            inEconomySmoothingComboBox.Items.Add("NO");
            inEconomySmoothingComboBox.Items.Add("1X");
            inEconomySmoothingComboBox.Items.Add("2X");
            inEconomySmoothingComboBox.Items.Add("3X");

            // Устанавливаем значения по умолчанию
            genderComboBox.SelectedIndex = 0;
            smoothingComboBox.SelectedIndex = 0;
            windowSizeNumericUpDown.Value = 5;
            inEconomySmoothingComboBox.SelectedIndex = 0;
            inEconomyWindowSizeNumericUpDown.Value = 5;

            // Добавление обработчиков событий
            genderComboBox.SelectedIndexChanged += GenderComboBox_SelectedIndexChanged;
            smoothingComboBox.SelectedIndexChanged += SmoothingComboBox_SelectedIndexChanged;
            windowSizeNumericUpDown.ValueChanged += WindowSizeNumericUpDown_ValueChanged;
            inEconomySmoothingComboBox.SelectedIndexChanged += InEconomySmoothingComboBox_SelectedIndexChanged;
            inEconomyWindowSizeNumericUpDown.ValueChanged += InEconomyWindowSizeNumericUpDown_ValueChanged;
        }

        private void GenderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            UpdateSmoothChart();
            UpdateInEconomyLevelChart();
        }

        private void SmoothingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            UpdateSmoothChart();
            UpdateInEconomyLevelChart();
        }

        private void WindowSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            _windowSize = (int)windowSizeNumericUpDown.Value;
            UpdateSmoothChart();
            UpdateInEconomyLevelChart();
        }

        private void NumericUpDownAge_ValueChanged(object sender, EventArgs e)
        {
            _minAge = (int)numericUpDownMinAge.Value;
            _maxAge = (int)numericUpDownMaxAge.Value;
            UpdateInEconomyLevelChart();
        }

        private void UpdateSmoothChart()
        {
            var regionId = CalculationStorage.Instance.CurrentRegion;
            var data = _repository.GetEconomyEmployedInRegion(regionId);

            var genderValue = (GenderComboBox)genderComboBox.SelectedIndex;
            var smoothingValue = (SmoothComboBox)smoothingComboBox.SelectedIndex;

            var economyEmploedSmoothData = _chartService.PrepareChartData(
                data,
                genderValue,
                _windowSize,
                (int)smoothingValue,
                useSmoothing: (int)smoothingValue > 0);

            PaintSmoothChart(economyEmploedSmoothData);
        }

        private void InEconomySmoothingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            UpdateInEconomyLevelSmoothChart();
        }

        private void InEconomyWindowSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            _inEconomyWindowSize = (int)((NumericUpDown)sender).Value;
            UpdateInEconomyLevelChart();
        }

        private void LoadSettings()
        {
            _isSetting = true;
            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            var settings = _repository.GetRegionEconomyEmploedFormSettings(regionId);
            if (settings != null)
            {
                // Загружаем настройки
                genderComboBox.SelectedIndex = settings.SelectedGender;
                smoothingComboBox.SelectedIndex = settings.SelectedSmoothing;
                windowSizeNumericUpDown.Value = settings.WindowSize;
                _windowSize = settings.WindowSize;

                inEconomySmoothingComboBox.SelectedIndex = settings.InEconomySelectedSmoothing;
                inEconomyWindowSizeNumericUpDown.Value = settings.InEconomyWindowSize;
                _inEconomyWindowSize = settings.InEconomyWindowSize;

                // Устанавливаем максимальные значения для осей Y графиков, если они были сохранены
                if (settings.EconomyEmploedMaxY > 0)
                    economyEmploed.ChartAreas[0].AxisY.Maximum = settings.EconomyEmploedMaxY;

                if (settings.EconomyEmploedSmoothMaxY > 0)
                    economyEmploedSmooth.ChartAreas[0].AxisY.Maximum = settings.EconomyEmploedSmoothMaxY;

                if (settings.InEconomyLevelMaxY > 0)
                    inEconomyLevel.ChartAreas[0].AxisY.Maximum = settings.InEconomyLevelMaxY;

                if (settings.InEconomyLevelSmoothMaxY > 0)
                    inEconomyLevelSmooth.ChartAreas[0].AxisY.Maximum = settings.InEconomyLevelSmoothMaxY;

                if(settings.InEconomyLevelMinAge > 12)
                    numericUpDownMinAge.Value = settings.InEconomyLevelMinAge;
                if (settings.InEconomyLevelMaxAge > 13)
                    numericUpDownMaxAge.Value = settings.InEconomyLevelMaxAge;
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

            var settings = new ForecastingWorkingPopulation.Database.Models.RegionEconomyEmploedFormSettingsEntity
            {
                RegionNumber = regionId,
                SelectedGender = genderComboBox.SelectedIndex,
                SelectedSmoothing = smoothingComboBox.SelectedIndex,
                WindowSize = (int)windowSizeNumericUpDown.Value,
                InEconomySelectedSmoothing = inEconomySmoothingComboBox.SelectedIndex,
                InEconomyWindowSize = (int)inEconomyWindowSizeNumericUpDown.Value,
                // Сохраняем максимальные значения для осей Y графиков
                EconomyEmploedMaxY = economyEmploed.ChartAreas[0].AxisY.Maximum,
                EconomyEmploedSmoothMaxY = economyEmploedSmooth.ChartAreas[0].AxisY.Maximum,
                InEconomyLevelMaxY = inEconomyLevel.ChartAreas[0].AxisY.Maximum,
                InEconomyLevelSmoothMaxY = inEconomyLevelSmooth.ChartAreas[0].AxisY.Maximum,
                InEconomyLevelMinAge = (int)numericUpDownMinAge.Value,
                InEconomyLevelMaxAge = (int)numericUpDownMaxAge.Value
            };

            _repository.SaveRegionEconomyEmploedFormSettings(settings);
        }

        private void UpdateInEconomyLevelSmoothChart()
        {
            // Настраиваем оси
            inEconomyLevelSmooth.Series.Clear();
            inEconomyLevelSmooth.ChartAreas[0].AxisX.Title = "Возраст";
            inEconomyLevelSmooth.ChartAreas[0].AxisY.Title = "Коэффициент занятости в экономике (сглаживание)";
            inEconomyLevelSmooth.ChartAreas[0].AxisX.Minimum = _minAge - 2;
            inEconomyLevelSmooth.ChartAreas[0].AxisY.Maximum = 1.6;

            var coefficentValues = CalculationStorage.Instance.GetInEconomyLevel();
            var visualizationValues = new List<RegionInEconomyLevelDto>();
            foreach (var value in coefficentValues)
                visualizationValues.Add(new RegionInEconomyLevelDto
                {
                    Age = value.Age,
                    Gender = value.Gender,
                    Level = value.Level,
                    Year = value.Year
                });
            
            var smoothingCount = inEconomySmoothingComboBox.SelectedIndex;
            if(smoothingCount > 0)
                visualizationValues = _smoothingCalculator.SmoothingValuesDto(visualizationValues, _inEconomyWindowSize, SmoothingType.MovingAverageWindow, smoothingCount);

            visualizationValues = visualizationValues.Where(x => x.Age <= _maxAge && x.Age >= _minAge).ToList();
            var maleValues = visualizationValues.Where(x => x.Gender == Gender.Male).OrderBy(x => x.Age);
            if (maleValues?.Any() == true)
                inEconomyLevelSmooth.Series.Add(_linearGraphPainter.PaintLinearGraph(
                    "Мужчины",
                    maleValues.Select(x => Convert.ToDouble(x.Age)).ToList(),
                    maleValues.Select(x => x.Level).ToList()));

            var femaleValues = visualizationValues.Where(x => x.Gender == Gender.Female).OrderBy(x => x.Age);
            if (femaleValues?.Any() == true)
                inEconomyLevelSmooth.Series.Add(_linearGraphPainter.PaintLinearGraph(
                    "Женщины",
                    femaleValues.Select(x => Convert.ToDouble(x.Age)).ToList(),
                    femaleValues.Select(x => x.Level).ToList()));


            CalculationStorage.Instance.StoreInEconomyForecastDataLevel(visualizationValues);
        }

        /// <summary>
        /// Устанавливает максимальное значение по оси Y для графика на основе данных серий
        /// </summary>
        private void SetChartYAxisMaximum(Chart chart, List<RegionStatisticsDto> data, string settingPropertyName)
        {
            // Находим максимальное значение Y среди всех серий
            double maxY = 0;
            maxY = data.Max(x => x.SummaryByYearSmoothed);

            // Устанавливаем максимальное значение оси Y с запасом 35%
            SetYAxisMaximum(chart, maxY, settingPropertyName);
        }

        /// <summary>
        /// Устанавливает максимальное значение по оси Y для графика с запасом 35%
        /// </summary>
        private void SetYAxisMaximum(Chart chart, double maxY, string settingPropertyName)
        {
            // Получаем текущие настройки
            int regionId = CalculationStorage.Instance.CurrentRegion;
            if (regionId <= 0) regionId = 10;

            var settings = _repository.GetRegionEconomyEmploedFormSettings(regionId);
            double savedMaxY = 0;

            // Получаем сохраненное максимальное значение, если оно есть
            if (settings != null)
            {
                switch (settingPropertyName)
                {
                    case "EconomyEmploedMaxY":
                        savedMaxY = settings.EconomyEmploedMaxY;
                        break;
                    case "EconomyEmploedSmoothMaxY":
                        savedMaxY = settings.EconomyEmploedSmoothMaxY;
                        break;
                    case "InEconomyLevelMaxY":
                        savedMaxY = settings.InEconomyLevelMaxY;
                        break;
                    case "InEconomyLevelSmoothMaxY":
                        savedMaxY = settings.InEconomyLevelSmoothMaxY;
                        break;
                }
            }

            // Увеличиваем максимальное значение на 35%
            double newMaxY = maxY * 1.35;

            // Используем большее из сохраненного и нового значения
            double finalMaxY = Math.Max(newMaxY, savedMaxY);

            // Устанавливаем максимальное значение оси Y
            if (finalMaxY > 0)
            {
                chart.ChartAreas[0].AxisY.Maximum = finalMaxY;
                chart.ChartAreas[0].RecalculateAxesScale();
            }
        }

        private void PaintChart(List<RegionStatisticsDto> data)
        {
            #region Занятые в экономике без сглаживания
            economyEmploed.Series.Clear();
            economyEmploed.ChartAreas[0].AxisX.Title = "Возраст";
            economyEmploed.ChartAreas[0].AxisY.Title = "Занятые в экономике";
            economyEmploed.ChartAreas[0].AxisX.Minimum = 0;
            #endregion
            var chartValues = data;
            chartValues = JoinGenderValueInOneAge(data);
            SetChartYAxisMaximum(economyEmploedSmooth, chartValues, "EconomyEmploedMaxY");
            var byYearGrouppedDto = chartValues.GroupBy(x => x.Year);
            foreach (var groupp in byYearGrouppedDto)
            {
                var values = groupp.OrderBy(x => x.Age).ToList();
                economyEmploed.Series.Add(_linearGraphPainter.PaintLinearGraph(groupp.Key.ToString(), 
                    values.Select(x => Convert.ToDouble(x.Age)).ToList(), 
                    values.Select(x => Convert.ToDouble(x.SummaryByYearSmoothed)).ToList()));
            }
        }

        private void PaintSmoothChart(List<RegionStatisticsDto> data)
        {
            #region Занятые в экономике со сглаживанием
            economyEmploedSmooth.Series.Clear();
            economyEmploedSmooth.ChartAreas[0].AxisX.Title = "Возраст";
            economyEmploedSmooth.ChartAreas[0].AxisY.Title = "Занятые в экономике (сглаживание)";
            economyEmploedSmooth.ChartAreas[0].AxisX.Minimum = 0;
            #endregion
            var chartValues = data;
            if ((GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.All)
                chartValues = JoinGenderValueInOneAge(data);
            SetChartYAxisMaximum(economyEmploedSmooth, chartValues, "EconomyEmploedMaxY");
            var byYearGrouppedDto = chartValues.GroupBy(x => x.Year);
            foreach (var groupp in byYearGrouppedDto)
            {
                var values = groupp.OrderBy(x => x.Age).ToList();
                economyEmploedSmooth.Series.Add(_linearGraphPainter.PaintLinearGraph(groupp.Key.ToString(),
                    values.Select(x => Convert.ToDouble(x.Age)).ToList(),
                    values.Select(x => Convert.ToDouble(x.SummaryByYearSmoothed)).ToList()));
                CalculationStorage.Instance.StoreEconomyEmploedForecastData(groupp.Key, data.Where(x => x.Year == groupp.Key).ToList());
            }
        }

        private void UpdateInEconomyLevelChart()
        {
            var regionId = CalculationStorage.Instance.CurrentRegion;
            var economyData = new List<RegionStatisticsDto>();
            economyData = CalculationStorage.Instance.GetEconomyEmploedForecastDataValues();
            if (!economyData.Any())
                economyData = _repository.GetEconomyEmployedInRegion(regionId);

            var populationData = new List<RegionStatisticsDto>();
            populationData = CalculationStorage.Instance.GetPermanentPopulationForecastDataValues();
            if (!populationData.Any())
                populationData = _repository.GetPopulationInRegion(regionId);

            // Очищаем график
            inEconomyLevel.Series.Clear();
            inEconomyLevel.ChartAreas[0].AxisX.Title = "Возраст";
            inEconomyLevel.ChartAreas[0].AxisY.Title = "Коэффициент занятости в экономике";
            inEconomyLevel.ChartAreas[0].AxisX.Minimum = _minAge - 2;
            inEconomyLevel.ChartAreas[0].AxisY.Maximum = 1.6;
            var maleValues = new List<RegionInEconomyLevelDto>();
            var femaleValues = new List<RegionInEconomyLevelDto>();
            var maleAverageValues = new List<RegionInEconomyLevelDto>();
            var femaleAverageValues = new List<RegionInEconomyLevelDto>();
            var years = populationData.Select(dto => dto.Year).Distinct();
            var ages = populationData.Select(dto => dto.Age).Distinct();
            var malesSeries = new Series("Мужчины") { ChartType = SeriesChartType.Line };
            var femalesSeries = new Series("Женщины") { ChartType = SeriesChartType.Line };
            foreach (var year in years)
            {
                malesSeries = new Series($"Мужчины {year}") { ChartType = SeriesChartType.Line };
                femalesSeries = new Series($"Женщины {year}") { ChartType = SeriesChartType.Line };
                foreach (var age in ages)
                {
                    var permanentValues = populationData.Where(dto => dto.Age == age && dto.Year == year);
                    foreach (var permanent in permanentValues)
                    {
                        var inEconomyValue = economyData
                            .FirstOrDefault(dto => dto.Year == year && permanent.Age == dto.Age && permanent.Gender == dto.Gender);
                        if (inEconomyValue is null)
                            continue;
                        var economyLevel = inEconomyValue.SummaryByYearSmoothed / permanent.SummaryByYearSmoothed;
                        if (economyLevel > 1.5)
                            continue;

                        if (permanent.Gender == Gender.Male)
                        {
                            malesSeries.Points.AddXY(permanent.Age, economyLevel);
                            maleValues.Add(new RegionInEconomyLevelDto
                            {
                                Age = permanent.Age,
                                Year = year,
                                Gender = Gender.Male,
                                Level = economyLevel
                            });
                        }
                        else
                        {
                            femalesSeries.Points.AddXY(permanent.Age, economyLevel);
                            femaleValues.Add(new RegionInEconomyLevelDto
                            {
                                Age = permanent.Age,
                                Year = year,
                                Gender = Gender.Female,
                                Level = economyLevel
                            });
                        }
                    }
                }
                if (!malesSeries.Points.Any() && !femalesSeries.Points.Any())
                    continue;

                if ((GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.All || (GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.Males)
                    inEconomyLevel.Series.Add(malesSeries);
                
                if ((GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.All || (GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.Females)
                    inEconomyLevel.Series.Add(femalesSeries);
            }

            malesSeries = new Series("Мужчины СР") { ChartType = SeriesChartType.Line, BorderWidth = 4 };
            malesSeries.Color = SetTransparency(Color.Blue, 190);
            femalesSeries = new Series("Женщины СР") { ChartType = SeriesChartType.Line, BorderWidth = 4 };
            femalesSeries.Color = SetTransparency(Color.DeepPink, 190);

            foreach (var age in ages)
            {
                var maleInEconomyByAge = maleValues.Where(value => value.Age == age);
                var femaleInEconomyByAge = femaleValues.Where(value => value.Age == age);
                var hasMaleValues = maleInEconomyByAge.Any();
                var hasFemaleValues = femaleInEconomyByAge.Any();
                if (!hasMaleValues && !hasFemaleValues)
                    continue;

                var maleAverageValue = 0.0;
                var femaleAverageValue = 0.0;

                if (hasMaleValues)
                    maleAverageValue = maleInEconomyByAge.Average(x => x.Level);
                if(hasFemaleValues)
                    femaleAverageValue = femaleInEconomyByAge.Average(x => x.Level);

                if (maleAverageValue > 1.5 || femaleAverageValue > 1.5)
                    continue;

                if (hasMaleValues)
                {
                    malesSeries.Points.AddXY(age, maleAverageValue);
                    maleAverageValues.Add(new RegionInEconomyLevelDto
                    {
                        Age = age,
                        Gender = Gender.Male,
                        Level = maleAverageValue
                    });
                }
                if (hasFemaleValues)
                {
                    femalesSeries.Points.AddXY(age, femaleAverageValue);
                    femaleAverageValues.Add(new RegionInEconomyLevelDto
                    {
                        Age = age,
                        Gender = Gender.Female,
                        Level = femaleAverageValue
                    });
                }
            }
            if ((GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.All || (GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.Males)
                inEconomyLevel.Series.Add(malesSeries);
            if ((GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.All || (GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.Females)
                inEconomyLevel.Series.Add(femalesSeries);

            maleAverageValues.AddRange(femaleAverageValues);
            // Сохраняем данные в хранилище
            CalculationStorage.Instance.StoreInEconomyLevel(maleAverageValues);

            // Обновляем график со сглаживанием, но только если это вызвано из конструктора или из обработчиков событий элементов управления графика уровня занятости
            UpdateInEconomyLevelSmoothChart();
        }

        private List<RegionStatisticsDto> JoinGenderValueInOneAge(List<RegionStatisticsDto> dtos)
        {
            var result = new List<RegionStatisticsDto>();
            foreach(var dto in dtos.Where(x => x.Gender == Gender.Male))
            {
                var femaleValue = dtos.FirstOrDefault(x => x.Year == dto.Year && x.Age == dto.Age && x.Gender == Gender.Female);
                result.Add(new RegionStatisticsDto
                {
                    Year = dto.Year,
                    Age = dto.Age,
                    SummaryByYearSmoothed = dto.SummaryByYearSmoothed + femaleValue?.SummaryByYearSmoothed ?? 0
                });
            }

            return result;
        }

        private Color SetTransparency(Color color, int alphaValue)
        {
            return Color.FromArgb(alphaValue, color);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки перед переходом на следующую форму
            SaveSettings();

            FormRouting.NextForm(2, this);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки перед переходом на следующую форму
            SaveSettings();

            FormRouting.PreviousForm(2, this);
        }
    }
}
