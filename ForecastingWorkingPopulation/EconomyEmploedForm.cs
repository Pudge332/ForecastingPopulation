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
            PaintCharts(economyEmploedData);

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
            SaveSettings();
            UpdateInEconomyLevelChart();
        }

        private void SmoothingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            UpdateSmoothChart();
            SaveSettings();
        }

        private void WindowSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            _windowSize = (int)windowSizeNumericUpDown.Value;
            UpdateSmoothChart();
            SaveSettings();
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
            SaveSettings();
        }

        private void InEconomyWindowSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_isSetting) return;
            _inEconomyWindowSize = (int)((NumericUpDown)sender).Value;
            UpdateInEconomyLevelSmoothChart();
            SaveSettings();
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
                InEconomyLevelSmoothMaxY = inEconomyLevelSmooth.ChartAreas[0].AxisY.Maximum
            };

            _repository.SaveRegionEconomyEmploedFormSettings(settings);
        }

        private void UpdateInEconomyLevelSmoothChart()
        {
            var regionId = CalculationStorage.Instance.CurrentRegion;
            var economyData = new List<RegionStatisticsDto>();
            //economyData = CalculationStorage.Instance.GetEconomyEmploedRegionStatisticsValuesSmoothed();
            //if (!economyData.Any())
            //    economyData = CalculationStorage.Instance.GetEconomyEmploedRegionStatisticsValues();
            if(!economyData.Any())
                economyData = _repository.GetEconomyEmployedInRegion(regionId);

            var populationData = new List<RegionStatisticsDto>();
            populationData = CalculationStorage.Instance.GetPermanentPopulationStatisticsValuesSmoothed();
            if (!populationData.Any())
                populationData = CalculationStorage.Instance.GetPermanentPopulationRegionStatisticsValues();
            if (!populationData.Any())
                populationData = _repository.GetPopulationInRegion(regionId);

            // Получаем значение сглаживания из комбобокса
            var smoothingValue = (SmoothComboBox)inEconomySmoothingComboBox.SelectedIndex;
            bool useSmoothing = (int)smoothingValue > 0;

            if(useSmoothing)
                for (int i = 0; i < (int)smoothingValue; i++)
                {
                    economyData = _smoothingCalculator.SmoothingValuesDto(economyData, _inEconomyWindowSize, SmoothingType.MovingAverageWindow);
                    populationData = _smoothingCalculator.SmoothingValuesDto(populationData, _inEconomyWindowSize, SmoothingType.MovingAverageWindow);
                }

            // Очищаем график
            inEconomyLevelSmooth.Series.Clear();
            // Создаем серии для мужчин и женщин
            var malesSeries = new Series("Мужчины" + (useSmoothing ? " (сглаж.)" : "")) { ChartType = SeriesChartType.Line };
            var femalesSeries = new Series("Женщины" + (useSmoothing ? " (сглаж.)" : "")) { ChartType = SeriesChartType.Line };

            // Группируем данные по возрасту и полу для расчета средних значений
            var economyByAgeAndGender = economyData
                .GroupBy(d => new { d.Age, d.Gender })
                .Select(g => new RegionStatisticsDto
                {
                    Age = g.Key.Age,
                    Gender = g.Key.Gender,
                    SummaryByYear = (int)g.Average(d => d.SummaryByYear),
                    SummaryByYearSmoothed = g.Average(d => d.SummaryByYearSmoothed)
                });

            var populationByAgeAndGender = populationData
                .GroupBy(d => new { d.Age, d.Gender })
                .Select(g => new RegionStatisticsDto
                {
                    Age = g.Key.Age,
                    Gender = g.Key.Gender,
                    SummaryByYear = (int)g.Average(d => d.SummaryByYear),
                    SummaryByYearSmoothed = g.Average(d => d.SummaryByYearSmoothed)
                });

            // Создаем списки коэффициентов для мужчин и женщин
            var maleCoefficients = new List<RegionStatisticsDto>();
            var femaleCoefficients = new List<RegionStatisticsDto>();

            // Объединяем данные и вычисляем коэффициенты
            foreach (var economy in economyByAgeAndGender)
            {
                var population = populationByAgeAndGender.FirstOrDefault(p =>
                    p.Age == economy.Age && p.Gender == economy.Gender);

                if (population != null && population.SummaryByYearSmoothed > 0)
                {
                    var coefficient = new RegionStatisticsDto
                    {
                        Age = economy.Age,
                        Gender = economy.Gender,
                        SummaryByYear = 0, // Не используем для коэффициентов
                        SummaryByYearSmoothed = economy.SummaryByYearSmoothed / population.SummaryByYearSmoothed
                    };

                    if (economy.Gender == Gender.Male)
                        maleCoefficients.Add(coefficient);
                    else
                        femaleCoefficients.Add(coefficient);
                }
            }

            // Сортируем по возрасту
            maleCoefficients = maleCoefficients.OrderBy(c => c.Age).ToList();
            femaleCoefficients = femaleCoefficients.OrderBy(c => c.Age).ToList();

            // Заполняем серии данными
            foreach (var coefficient in maleCoefficients)
            {
                malesSeries.Points.AddXY(coefficient.Age, coefficient.SummaryByYearSmoothed);
            }

            foreach (var coefficient in femaleCoefficients)
            {
                femalesSeries.Points.AddXY(coefficient.Age, coefficient.SummaryByYearSmoothed);
            }

            // Добавляем серии на график
            inEconomyLevelSmooth.Series.Add(malesSeries);
            inEconomyLevelSmooth.Series.Add(femalesSeries);


            // Настраиваем оси
            inEconomyLevelSmooth.ChartAreas[0].AxisX.Title = "Возраст";
            inEconomyLevelSmooth.ChartAreas[0].AxisY.Title = "Коэффициент занятости в экономике (сглаживание)";
            inEconomyLevelSmooth.ChartAreas[0].AxisX.Minimum = 0;

            // Устанавливаем максимальное значение по оси Y
            double maxY = Math.Max(
                maleCoefficients.Count > 0 ? maleCoefficients.Max(c => c.SummaryByYearSmoothed) : 0,
                femaleCoefficients.Count > 0 ? femaleCoefficients.Max(c => c.SummaryByYearSmoothed) : 0
            );
            SetYAxisMaximum(inEconomyLevelSmooth, maxY, "InEconomyLevelSmoothMaxY");

            // Сохраняем данные в CalculationStorage
            var maleCoefficientsData = maleCoefficients.Select(c => new RegionInEconomyLevelDto
            {
                Age = c.Age,
                Gender = c.Gender,
                Level = c.SummaryByYearSmoothed
            }).ToList();

            var femaleCoefficientsData = femaleCoefficients.Select(c => new RegionInEconomyLevelDto
            {
                Age = c.Age,
                Gender = c.Gender,
                Level = c.SummaryByYearSmoothed
            }).ToList();

            //Объединяем все данные 
            maleCoefficientsData.AddRange(femaleCoefficientsData);

            // Сохраняем данные в хранилище
            CalculationStorage.Instance.StoreInEconomyLevel(maleCoefficientsData);
        }

        /// <summary>
        /// Устанавливает максимальное значение по оси Y для графика на основе данных серий
        /// </summary>
        private void SetChartYAxisMaximum(Chart chart, List<ChartDataService.SeriesData> data, string settingPropertyName)
        {
            // Находим максимальное значение Y среди всех серий
            double maxY = 0;
            foreach (var seriesData in data)
            {
                if (seriesData.YValues.Count > 0)
                {
                    double seriesMaxY = seriesData.YValues.Max();
                    maxY = Math.Max(maxY, seriesMaxY);
                }
            }

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

        private void PaintCharts(List<ChartDataService.SeriesData> data)
        {
            #region Занятые в экономике без сглаживания
            economyEmploed.Series.Clear();
            foreach (var byYear in data.GroupBy(x => x.SeriesName))
                economyEmploed.Series.Add(_linearGraphPainter.PainLinearGraph(
                    name: byYear.Key,
                    xValues: byYear.SelectMany(x => x.XValues).ToList(),
                    yValues: byYear.SelectMany(x => x.YValues).ToList()));

            economyEmploed.ChartAreas[0].AxisX.Title = "Возраст";
            economyEmploed.ChartAreas[0].AxisY.Title = "Занятые в экономике";
            economyEmploed.ChartAreas[0].AxisX.Minimum = 0;

            // Устанавливаем максимальное значение по оси Y
            SetChartYAxisMaximum(economyEmploed, data, "EconomyEmploedMaxY");
            #endregion

            // Сохраняем данные в CalculationStorage
            foreach (var seriesData in data)
            {
                var statisticsData = new List<RegionStatisticsDto>();
                for (int i = 0; i < seriesData.XValues.Count; i++)
                {
                    statisticsData.Add(new RegionStatisticsDto
                    {
                        Age = (int)seriesData.XValues[i],
                        SummaryByYear = (int)seriesData.YValues[i],
                        Gender = seriesData.SeriesName.Contains("Мужчины") ? Models.Enums.Gender.Male : Models.Enums.Gender.Female
                    });
                }
                CalculationStorage.Instance.StoreEconomyEmploedRegionStatistics(int.Parse(seriesData.SeriesName), statisticsData);
            }
        }

        private void PaintSmoothChart(List<ChartDataService.SeriesData> data)
        {
            #region Занятые в экономике со сглаживанием
            economyEmploedSmooth.Series.Clear();
            foreach (var byYear in data.GroupBy(x => x.SeriesName))
                economyEmploedSmooth.Series.Add(_linearGraphPainter.PainLinearGraph(
                    name: byYear.Key,
                    xValues: byYear.SelectMany(x => x.XValues).ToList(),
                    yValues: byYear.SelectMany(x => x.YValues).ToList()));

            economyEmploedSmooth.ChartAreas[0].AxisX.Title = "Возраст";
            economyEmploedSmooth.ChartAreas[0].AxisY.Title = "Занятые в экономике (сглаживание)";
            economyEmploedSmooth.ChartAreas[0].AxisX.Minimum = 0;

            // Устанавливаем максимальное значение по оси Y
            SetChartYAxisMaximum(economyEmploedSmooth, data, "EconomyEmploedSmoothMaxY");
            #endregion

            // Сохраняем данные в CalculationStorage
            foreach (var seriesData in data)
            {
                var statisticsData = new List<RegionStatisticsDto>();
                for (int i = 0; i < seriesData.XValues.Count; i++)
                {
                    statisticsData.Add(new RegionStatisticsDto
                    {
                        Age = (int)seriesData.XValues[i],
                        SummaryByYear = (int)seriesData.YValues[i],
                        SummaryByYearSmoothed = seriesData.YValues[i],
                        Gender = seriesData.SeriesName.Contains("Мужчины") ? Models.Enums.Gender.Male : Models.Enums.Gender.Female
                    });
                }
                CalculationStorage.Instance.StoreEconomyEmploedRegionStatisticsSmoothed(int.Parse(seriesData.SeriesName), statisticsData);
            }
        }

        private void UpdateInEconomyLevelChart(bool yea)
        {
            var regionId = CalculationStorage.Instance.CurrentRegion;
            var economyData = new List<RegionStatisticsDto>();
            //economyData = CalculationStorage.Instance.GetEconomyEmploedRegionStatisticsValuesSmoothed();
            //if (!economyData.Any())
            //   economyData = CalculationStorage.Instance.GetEconomyEmploedRegionStatisticsValues();
            if (!economyData.Any())
                economyData = _repository.GetEconomyEmployedInRegion(regionId);

            var populationData = new List<RegionStatisticsDto>();
            populationData = CalculationStorage.Instance.GetPermanentPopulationStatisticsValuesSmoothed();
            if (!populationData.Any())
                populationData = CalculationStorage.Instance.GetPermanentPopulationRegionStatisticsValues();
            if (!populationData.Any())
                populationData = _repository.GetPopulationInRegion(regionId);

            // Очищаем график
            inEconomyLevel.Series.Clear();

            // Создаем серии для мужчин и женщин
            var malesSeries = new Series("Мужчины") { ChartType = SeriesChartType.Line };
            var femalesSeries = new Series("Женщины") { ChartType = SeriesChartType.Line };

                // Группируем данные по возрасту и полу для расчета средних значений
                var economyByAgeAndGender = economyData
                    .GroupBy(d => new { d.Age, d.Gender })
                    .Select(g => new
                    {
                        Age = g.Key.Age,
                        Gender = g.Key.Gender,
                        AverageEconomy = g.Average(d => d.SummaryByYear)
                    });

                var populationByAgeAndGender = populationData
                    .GroupBy(d => new { d.Age, d.Gender })
                    .Select(g => new
                    {
                        Age = g.Key.Age,
                        Gender = g.Key.Gender,
                        AveragePopulation = g.Average(d => d.SummaryByYear)
                    });

                // Объединяем данные и вычисляем коэффициенты
                var coefficients = economyByAgeAndGender
                    .Join(populationByAgeAndGender,
                        e => new { e.Age, e.Gender },
                        p => new { p.Age, p.Gender },
                        (e, p) => new
                        {
                            Age = e.Age,
                            Gender = e.Gender,
                            Coefficient = p.AveragePopulation > 0 ? e.AverageEconomy / p.AveragePopulation : 0
                        })
                    .OrderBy(c => c.Age)
                    .ToList();

                // Заполняем серии данными
                foreach (var item in coefficients.Where(c => c.Gender == Gender.Male))
                {
                    malesSeries.Points.AddXY(item.Age, item.Coefficient);
                }

                foreach (var item in coefficients.Where(c => c.Gender == Gender.Female))
                {
                    femalesSeries.Points.AddXY(item.Age, item.Coefficient);
                }

                // Добавляем серии на график
                inEconomyLevel.Series.Add(malesSeries);
                inEconomyLevel.Series.Add(femalesSeries);
            
            // Настраиваем оси
            inEconomyLevel.ChartAreas[0].AxisX.Title = "Возраст";
            inEconomyLevel.ChartAreas[0].AxisY.Title = "Коэффициент занятости в экономике";
            inEconomyLevel.ChartAreas[0].AxisX.Minimum = 0;

            // Устанавливаем максимальное значение по оси Y
            double maxY = 0;
            if (coefficients.Any())
            {
                var maleMax = coefficients.Where(c => c.Gender == Gender.Male).Any() ?
                    coefficients.Where(c => c.Gender == Gender.Male).Max(c => c.Coefficient) : 0;
                var femaleMax = coefficients.Where(c => c.Gender == Gender.Female).Any() ?
                    coefficients.Where(c => c.Gender == Gender.Female).Max(c => c.Coefficient) : 0;
                maxY = Math.Max(maleMax, femaleMax);
            }
            SetYAxisMaximum(inEconomyLevel, maxY, "InEconomyLevelMaxY");

            // Сохраняем данные в CalculationStorage
            var coefficientsData = coefficients.Select(c => new RegionInEconomyLevelDto
            {
                Age = c.Age,
                Gender = c.Gender,
                Level = c.Coefficient
            }).ToList();

            // Сохраняем данные в хранилище
            CalculationStorage.Instance.StoreInEconomyLevel(coefficientsData);

            // Обновляем график со сглаживанием, но только если это вызвано из конструктора или из обработчиков событий элементов управления графика уровня занятости
            UpdateInEconomyLevelSmoothChart();
        }

        private void UpdateInEconomyLevelChart()
        {
            var regionId = CalculationStorage.Instance.CurrentRegion;
            var economyData = new List<RegionStatisticsDto>();
            //economyData = CalculationStorage.Instance.GetEconomyEmploedRegionStatisticsValuesSmoothed();
            //if (!economyData.Any())
            //   economyData = CalculationStorage.Instance.GetEconomyEmploedRegionStatisticsValues();
            if (!economyData.Any())
                economyData = _repository.GetEconomyEmployedInRegion(regionId);

            var populationData = new List<RegionStatisticsDto>();
            //populationData = CalculationStorage.Instance.GetPermanentPopulationStatisticsValuesSmoothed();
            //if (!populationData.Any())
            //    populationData = CalculationStorage.Instance.GetPermanentPopulationRegionStatisticsValues();
            if (!populationData.Any())
                populationData = _repository.GetPopulationInRegion(regionId);

            // Очищаем график
            inEconomyLevel.Series.Clear();
            inEconomyLevel.ChartAreas[0].AxisX.Title = "Возраст";
            inEconomyLevel.ChartAreas[0].AxisY.Title = "Коэффициент занятости в экономике";
            inEconomyLevel.ChartAreas[0].AxisX.Minimum = 0;
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

                        if (permanent.Gender == Gender.Male)
                        {
                            malesSeries.Points.AddXY(permanent.Age, inEconomyValue.SummaryByYear / (double)permanent.SummaryByYear);
                            maleValues.Add(new RegionInEconomyLevelDto
                            {
                                Age = permanent.Age,
                                Year = year,
                                Gender = Gender.Male,
                                Level = inEconomyValue.SummaryByYear / (double)permanent.SummaryByYear
                            });
                        }
                        else
                        {
                            femalesSeries.Points.AddXY(permanent.Age, inEconomyValue.SummaryByYear / (double)permanent.SummaryByYear);
                            femaleValues.Add(new RegionInEconomyLevelDto
                            {
                                Age = permanent.Age,
                                Year = year,
                                Gender = Gender.Female,
                                Level = inEconomyValue.SummaryByYear / (double)permanent.SummaryByYear
                            });
                        }
                    }
                }
                if (!malesSeries.Points.Any() || !femalesSeries.Points.Any())
                    continue;

                if((GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.All || (GenderComboBox)genderComboBox.SelectedIndex == GenderComboBox.Males)
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
                if (!maleInEconomyByAge.Any() || !femaleInEconomyByAge.Any())
                    continue;

                var maleAverageValue = maleInEconomyByAge.Average(x => x.Level);
                var femaleAverageValue = femaleInEconomyByAge.Average(x => x.Level);
                malesSeries.Points.AddXY(age, maleAverageValue);
                femalesSeries.Points.AddXY(age, femaleAverageValue);
                maleAverageValues.Add(new RegionInEconomyLevelDto
                {
                    Age = age,
                    Gender = Gender.Male,
                    Level = maleAverageValue
                });
                femaleAverageValues.Add(new RegionInEconomyLevelDto
                {
                    Age = age,
                    Gender = Gender.Female,
                    Level = femaleAverageValue
                });
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

        private Color SetTransparency(Color color, int alphaValue)
        {
            return Color.FromArgb(alphaValue, color);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки перед переходом на предыдущую форму
            SaveSettings();

            // Закрываем текущую форму
            this.Close();
            // Предыдущая форма (PermanentPopulationForm) будет показана автоматически
            // благодаря обработчику FormClosed в PermanentPopulationForm
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки перед переходом на следующую форму
            SaveSettings();

            // Открываем форму ForecastionForm
            var forecastForm = new ForecastionForm();
            forecastForm.Show();

            // Скрываем текущую форму
            this.Hide();

            // Добавляем обработчик закрытия формы ForecastionForm
            forecastForm.FormClosed += (s, args) => this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки перед переходом на следующую форму
            SaveSettings();

            // Открываем форму EconomyEmploedForm
            var forecastForm = new ForecastionForm();
            forecastForm.Show();

            // Скрываем текущую форму
            this.Hide();

            // Добавляем обработчик закрытия формы EconomyEmploedForm
            forecastForm.FormClosed += (s, args) => this.Show();
        }
    }
}
