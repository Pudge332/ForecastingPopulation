using System.Data;
using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Infrastructure;
using ForecastingWorkingPopulation.Models.Enums;
using System.Windows.Forms.DataVisualization.Charting;
using ForecastingWorkingPopulation.Infrastructure.Excel;

namespace ForecastingWorkingPopulation
{
    public partial class PermanentPopulationForm : Form
    {
        private readonly IPopulationRepository _populationRepository;
        private readonly LinearGraphPainter _painter;
        private readonly SmoothingCalculator _smoothingCalculator;
        private readonly IExcelParser _excelParser;

        private bool _isSetting = false;
        private int _windowSize = 5; // Значение по умолчанию для размера окна сглаживания
        private decimal _maxCoefficentValue = 1;
        private int _minAge = 0;
        private int _maxAge = 80;

        private Dictionary<int, YearControl> _yearControls = new Dictionary<int, YearControl>();

        public PermanentPopulationForm()
        {
            InitializeComponent();
            _populationRepository = new PopulationRepository();
            _painter = new LinearGraphPainter();
            _smoothingCalculator = new SmoothingCalculator();
            _excelParser = new ExcelParser();
            Init();

            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            // Инициализируем элементы управления для графика постоянного населения
            InitComboboxes();
            LoadSettings();

            // Загружаем настройки коэффициентов для текущего региона
            CalculateAndPaintCoefficent(regionId);
            LoadRegionCoefficientSettings(regionId);
            CalculateAndPaintCoefficent(regionId);

            // Отрисовываем график постоянного населения
            PaintByGender((GenderComboBox)genderComboBox.SelectedIndex, (SmoothComboBox)smoothingComboBox.SelectedIndex);

            // Создаем прогноз численности постоянного населения по годам
            CreatePopulationForecastByYeart(regionId);
        }

        private void Init()
        {
            label3.Text = "Минимальный возраст";
            label4.Text = "Максимальный возраст";
            numericUpDown2.Value = _minAge;
            numericUpDown3.Value = _maxAge;
        }

        private void InitComboboxes()
        {
            genderComboBox.Items.Add("Все");
            genderComboBox.Items.Add("Мужчины");
            genderComboBox.Items.Add("Женщины");
            smoothingComboBox.Items.Add("NO");
            smoothingComboBox.Items.Add("1X");
            smoothingComboBox.Items.Add("2X");
            smoothingComboBox.Items.Add("3X");

            genderComboBox.SelectedIndex = 0;
            smoothingComboBox.SelectedIndex = 0;
            genderComboBox.SelectedIndexChanged += GenderComboBoxChanged;
            smoothingComboBox.SelectedIndexChanged += SmoothingComboBoxChanged;
            windowSizeNumericUpDown.ValueChanged += WindowSizeNumericUpDown_ValueChanged;
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
                genderComboBox.SelectedIndex = settings.SelectedGender;
                smoothingComboBox.SelectedIndex = settings.SelectedSmoothing;
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
                SelectedGender = genderComboBox.SelectedIndex,
                SelectedSmoothing = smoothingComboBox.SelectedIndex,
                WindowSize = (int)windowSizeNumericUpDown.Value
            };

            // Сохраняем текущее максимальное значение оси Y, если оно установлено
            if (PermanentPopulation.ChartAreas.Count > 0 && PermanentPopulation.ChartAreas[0].AxisY.Maximum != double.NaN)
            {
                var existingSettings = _populationRepository.GetRegionMainFormSettings(regionId);
                if (existingSettings != null && existingSettings.PermanentPopulationMaxY > 0)
                {
                    settings.PermanentPopulationMaxY = existingSettings.PermanentPopulationMaxY;
                }
            }

            _populationRepository.SaveRegionMainFormSettings(settings);
        }

        private void SmoothingComboBoxChanged(object sender, EventArgs e)
        {
            if (_isSetting)
                return;

            PaintByGender((GenderComboBox)genderComboBox.SelectedIndex, (SmoothComboBox)smoothingComboBox.SelectedIndex);
            SaveSettings();
        }

        private void GenderComboBoxChanged(object sender, EventArgs e)
        {
            if (_isSetting)
                return;

            PaintByGender((GenderComboBox)genderComboBox.SelectedIndex, (SmoothComboBox)smoothingComboBox.SelectedIndex);
            SaveSettings();
        }

        private void WindowSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_isSetting)
                return;

            _windowSize = (int)windowSizeNumericUpDown.Value;
            PaintByGender((GenderComboBox)genderComboBox.SelectedIndex, (SmoothComboBox)smoothingComboBox.SelectedIndex);
            SaveSettings();
        }

        private void PaintByGender(GenderComboBox genderComboValue, SmoothComboBox smoothComboValue)
        {
            PermanentPopulation.Series.Clear();

            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            var populationDtos = _populationRepository.GetPopulationInRegion(regionId);

            // Обновляем заголовок формы, чтобы показать текущий регион
            this.Text = $"Постоянное население региона (ID: {regionId})";

            PaintChartData(populationDtos, genderComboValue, smoothComboValue, PermanentPopulation);
        }

        private void PaintChartData(List<RegionStatisticsDto> dtos, GenderComboBox genderComboValue, SmoothComboBox smoothComboValue, Chart currentChart)
        {
            var groupedByYear = dtos
                .GroupBy(dto => dto.Year);

            // Создаем список для хранения данных серий для последующего расчета максимального значения Y
            var seriesDataList = new List<ChartDataService.SeriesData>();

            foreach (var group in groupedByYear)
            {
                var currentGroup = SelectByGender(genderComboValue, group);
                var xValues = currentGroup.Select(dto => dto.Age).Select(Convert.ToDouble).ToList();
                var yValues = currentGroup.Select(dto => dto.SummaryByYear).Select(Convert.ToDouble).ToList();

                yValues = _smoothingCalculator.SmoothingValues(currentGroup, windowSize: _windowSize, SmoothingType.MovingAverageWindow, (int)smoothComboValue);
                CalculationStorage.Instance.StorePermanentPopulationForecastData(group.Key, currentGroup.ToList());

                if (genderComboValue == GenderComboBox.All)
                {
                    xValues = xValues.Distinct().ToList();
                    var resultY = new List<double>();
                    for (int i = 0; i < yValues.Count; i += 2)
                    {
                        resultY.Add(yValues[i] + yValues[i + 1]);
                    }
                    yValues = resultY;
                }

                var series = _painter.PainLinearGraph($"Год {group.Key}", xValues, yValues);
                currentChart.Series.Add(series);

                // Добавляем данные серии в список для расчета максимального значения Y
                seriesDataList.Add(new ChartDataService.SeriesData
                {
                    SeriesName = $"Год {group.Key}",
                    XValues = xValues,
                    YValues = yValues
                });
            }

            currentChart.ChartAreas[0].AxisX.Title = "Возраст";
            currentChart.ChartAreas[0].AxisY.Title = "Численность населения в тысячах";
            currentChart.ChartAreas[0].AxisX.Minimum = 0;

            // Устанавливаем максимальное значение по оси Y
            SetChartYAxisMaximum(currentChart, seriesDataList, "PermanentPopulationMaxY");
            CreatePopulationForecastByYeart(CalculationStorage.Instance.CurrentRegion);
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

            var settings = _populationRepository.GetRegionMainFormSettings(regionId);
            double savedMaxY = 0;

            // Получаем сохраненное максимальное значение, если оно есть
            if (settings != null && settingPropertyName == "PermanentPopulationMaxY")
            {
                savedMaxY = settings.PermanentPopulationMaxY;
            }

            // Увеличиваем максимальное значение на 35%
            double newMaxY = maxY * 1.35;

            // Используем большее из сохраненного и нового значения
            double finalMaxY = newMaxY;

            // Устанавливаем максимальное значение оси Y
            if (finalMaxY > 0)
            {
                chart.ChartAreas[0].AxisY.Maximum = finalMaxY;
                chart.ChartAreas[0].RecalculateAxesScale();

                // Сохраняем новое значение в настройки
                if (settings != null)
                {
                    if (settingPropertyName == "PermanentPopulationMaxY")
                    {
                        settings.PermanentPopulationMaxY = finalMaxY;
                        _populationRepository.SaveRegionMainFormSettings(settings);
                    }
                }
            }
        }

        private IEnumerable<RegionStatisticsDto> SelectByGender(GenderComboBox comboValue, IEnumerable<RegionStatisticsDto> dtos)
        {
            switch (comboValue)
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

        private void CalculateAndPaintCoefficent(int regionNumber)
        {
            lifeExpectancyCoefficient.Series.Clear();
            var chartArea = lifeExpectancyCoefficient.ChartAreas[0];
            chartArea.AxisY.Maximum = 1.5;
            var ages = new List<double>();
            var dtos = new List<RegionStatisticsDto>(CalculationStorage.Instance.GetPermanentPopulationRegionStatisticsValues());
            if (!dtos.Any())
                dtos = _populationRepository.GetPopulationInRegion(regionNumber);
            var years = dtos.Select(dto => dto.Year).Distinct().OrderBy(x => x);

            // Обновляем заголовок формы, чтобы показать текущий регион
            this.Text = $"Постоянное население региона (ID: {regionNumber})";

            // Отдельные списки для мужчин и женщин
            var coefficentDtosMale = new List<RegionCoefficentDto>();
            var coefficentDtosFemale = new List<RegionCoefficentDto>();

            foreach (var year in years)
            {
                if (year == years.Last())
                    continue;

                // Получаем данные для мужчин
                var maleData = GetData(
                    dtos.Where(dto => dto.Year == year && dto.Gender == Gender.Male),
                    dtos.Where(dto => dto.Year == year + 1 && dto.Gender == Gender.Male));
                coefficentDtosMale.AddRange(maleData);

                // Получаем данные для женщин
                var femaleData = GetData(
                    dtos.Where(dto => dto.Year == year && dto.Gender == Gender.Female),
                    dtos.Where(dto => dto.Year == year + 1 && dto.Gender == Gender.Female));
                coefficentDtosFemale.AddRange(femaleData);
            }

            // Обрабатываем данные для мужчин
            var grouppedDtosMale = GetAverage(coefficentDtosMale);
            // Устанавливаем пол для всех записей
            grouppedDtosMale.ForEach(dto => dto.Gender = Gender.Male);

            // Обрабатываем данные для женщин
            var grouppedDtosFemale = GetAverage(coefficentDtosFemale);
            // Устанавливаем пол для всех записей
            grouppedDtosFemale.ForEach(dto => dto.Gender = Gender.Female);

            // Создаем серию для мужчин
            var xValuesMale = new List<double>();
            var yValuesMale = new List<double>();

            foreach (var group in grouppedDtosMale)
            {
                xValuesMale.Add(group.Age);
                yValuesMale.Add(group.Coefficent);
            }

            var seriesMale = _painter.PainLinearGraph("КПЖ (мужчины)", xValuesMale, yValuesMale);
            seriesMale.Color = System.Drawing.Color.Blue;
            lifeExpectancyCoefficient.Series.Add(seriesMale);
            lifeExpectancyCoefficient.ChartAreas[0].AxisX.Minimum = 0;

            // Создаем серию для женщин
            var xValuesFemale = new List<double>();
            var yValuesFemale = new List<double>();

            foreach (var group in grouppedDtosFemale)
            {
                xValuesFemale.Add(group.Age);
                yValuesFemale.Add(group.Coefficent);
            }

            var seriesFemale = _painter.PainLinearGraph("КПЖ (женщины)", xValuesFemale, yValuesFemale);
            seriesFemale.Color = System.Drawing.Color.Red;
            lifeExpectancyCoefficient.Series.Add(seriesFemale);

            // Сохраняем данные отдельно для мужчин и женщин
            CalculationStorage.Instance.StoreLifeExpectancyCalculation(grouppedDtosMale, Gender.Male);
            CalculationStorage.Instance.StoreLifeExpectancyCalculation(grouppedDtosFemale, Gender.Female);
            CalculationStorage.Instance.StoreAvailableYears(years.ToList());
            CreateYearControls();
        }

        private void CreateYearControls()
        {
            var startX = 10;
            var startY = lifeExpectancyCoefficient.Bottom + 10;

            // Clear existing controls if the number of years has changed
            if (_yearControls.Count != CalculationStorage.Instance.GetAvailableYears().Count)
            {
                foreach (var control in _yearControls.Values)
                {
                    this.Controls.Remove(control.YearLabel);
                    control.YearNumericUpDown.ValueChanged -= YearNumerics_ValueChanged;
                    this.Controls.Remove(control.YearNumericUpDown);
                }
                _yearControls.Clear();
            }

            foreach (var year in CalculationStorage.Instance.GetAvailableYears())
            {
                if (!_yearControls.ContainsKey(year))
                {
                    var yearControl = new YearControl(year, new Point(startX, startY));
                    this.Controls.Add(yearControl.YearLabel);
                    this.Controls.Add(yearControl.YearNumericUpDown);
                    yearControl.YearNumericUpDown.ValueChanged += YearNumericUpDown_ValueChanged;
                    _yearControls[year] = yearControl;

                    startX += 70; // Spacing between year controls
                }
            }
        }

        private void YearNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            CalculateCoefficents();
        }

        private Dictionary<int, double> GetUsersCoefficents()
        {
            var coefficents = new Dictionary<int, double>();
            foreach (var controll in _yearControls)
                coefficents.Add(controll.Key, Convert.ToDouble(controll.Value.YearNumericUpDown.Value));

            return coefficents;
        }

        private double GetMaxCoefficentValue()
        {
            return (double)_maxCoefficentValue;
        }

        private List<RegionCoefficentDto> GetAverage(List<RegionCoefficentDto> dtos)
        {
            var coefficents = new List<RegionCoefficentDto>();
            var maxCoefficent = GetMaxCoefficentValue();

            foreach (var group in dtos.GroupBy(dto => dto.Age))
            {
                var byAge = group.ToList();
                var coefficent = GetWeightedCoefficient(byAge);
                if (coefficent > maxCoefficent)
                    coefficent = maxCoefficent;

                if (coefficent == 0)
                    continue;

                coefficents.Add(new RegionCoefficentDto
                {
                    Age = group.Key,
                    Coefficent = coefficent
                });
            }

            return coefficents;
        }

        private double GetWeightedCoefficient(List<RegionCoefficentDto> dtos)
        {
            var weights = GetUsersCoefficents();
            if (!weights.Any())
                return dtos.Sum(dto => dto.Coefficent) / dtos.Count();

            var coefficent = 0.0;
            var divisor = 0.0;
            foreach (var dto in dtos)
            {
                var weight = weights[dto.Year];
                if (!weights.TryGetValue(dto.Year, out weight))
                {
                    coefficent += dto.Coefficent;
                    continue;
                }

                coefficent += dto.Coefficent * weight;
                divisor += weight;
            }

            return coefficent / divisor;
        }

        private List<RegionCoefficentDto> GetData(IEnumerable<RegionStatisticsDto> currentYearDtos, IEnumerable<RegionStatisticsDto> nextYearDtos)
        {
            var coefficents = new List<RegionCoefficentDto>();
            var maxCoefficent = GetMaxCoefficentValue();

            foreach (var currentYearDto in currentYearDtos)
            {
                var nextYearDto = nextYearDtos.FirstOrDefault(dto => dto.Age == currentYearDto.Age + 1 && dto.Gender == currentYearDto.Gender);
                if (currentYearDto.SummaryByYearSmoothed < 1 || nextYearDto == null)
                    continue;

                var coefficent = (double)nextYearDto.SummaryByYearSmoothed / currentYearDto.SummaryByYearSmoothed;

                if (coefficent > maxCoefficent)
                    coefficent = maxCoefficent;

                coefficents.Add(new RegionCoefficentDto
                {
                    Year = currentYearDto.Year,
                    Age = currentYearDto.Age,
                    Coefficent = coefficent,
                    Gender = currentYearDto.Gender
                });
            }

            return coefficents.Where(coefficent => coefficent.Age >= _minAge && coefficent.Age <= _maxAge).ToList();
        }

        private void LoadRegionCoefficientSettings(int regionNumber)
        {
            var settings = _populationRepository.GetRegionCoefficientSettings(regionNumber);
            if (settings == null)
                return;

            // Устанавливаем значения из настроек
            _minAge = settings.MinAge;
            _maxAge = settings.MaxAge;
            numericUpDown1.Value = settings.CoefficientLimit;
            //checkBox1.Checked = settings.DisableCoefficientCutoff;
            numericUpDown2.Value = settings.MinAge;
            numericUpDown3.Value = settings.MaxAge;

            // Устанавливаем коэффициенты для годов
            if (_yearControls.ContainsKey(2019) && settings.Coefficient2019 >= 0)
                _yearControls[2019].YearNumericUpDown.Value = CheckValue((decimal)settings.Coefficient2019, 0, 1);
            if (_yearControls.ContainsKey(2020) && settings.Coefficient2020 >= 0)
                _yearControls[2020].YearNumericUpDown.Value = CheckValue((decimal)settings.Coefficient2020, 0, 1);
            if (_yearControls.ContainsKey(2021) && settings.Coefficient2021 >= 0)
                _yearControls[2021].YearNumericUpDown.Value = CheckValue((decimal)settings.Coefficient2021, 0, 1);
            if (_yearControls.ContainsKey(2022) && settings.Coefficient2022 >= 0)
                _yearControls[2022].YearNumericUpDown.Value = CheckValue((decimal)settings.Coefficient2022, 0, 1);
            if (_yearControls.ContainsKey(2023) && settings.Coefficient2023 >= 0)
                _yearControls[2023].YearNumericUpDown.Value = CheckValue((decimal)settings.Coefficient2023, 0, 1);
            if (_yearControls.ContainsKey(2024) && settings.Coefficient2024 >= 0)
                _yearControls[2024].YearNumericUpDown.Value = CheckValue((decimal)settings.Coefficient2024, 0, 1);
        }

        private decimal CheckValue(decimal value, decimal min, decimal max)
        {
            if (value <= max && value >= min)
                return value;

            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }

        private void SaveRegionCoefficientSettings(int regionNumber)
        {
            var settings = new ForecastingWorkingPopulation.Database.Models.RegionCoefficientSettingsEntity
            {
                RegionNumber = regionNumber,
                MinAge = (int)numericUpDown2.Value,
                MaxAge = (int)numericUpDown3.Value,
                CoefficientLimit = numericUpDown1.Value,
                //DisableCoefficientCutoff = checkBox1.Checked
            };

            // Сохраняем коэффициенты для годов
            if (_yearControls.ContainsKey(2019))
                settings.Coefficient2019 = Convert.ToDouble(_yearControls[2019].YearNumericUpDown.Value);
            if (_yearControls.ContainsKey(2020))
                settings.Coefficient2020 = Convert.ToDouble(_yearControls[2020].YearNumericUpDown.Value);
            if (_yearControls.ContainsKey(2021))
                settings.Coefficient2021 = Convert.ToDouble(_yearControls[2021].YearNumericUpDown.Value);
            if (_yearControls.ContainsKey(2022))
                settings.Coefficient2022 = Convert.ToDouble(_yearControls[2022].YearNumericUpDown.Value);
            if (_yearControls.ContainsKey(2023))
                settings.Coefficient2023 = Convert.ToDouble(_yearControls[2023].YearNumericUpDown.Value);
            if (_yearControls.ContainsKey(2024))
                settings.Coefficient2024 = Convert.ToDouble(_yearControls[2024].YearNumericUpDown.Value);

            _populationRepository.SaveRegionCoefficientSettings(settings);
        }

        private void CalculateCoefficents()
        {
            _minAge = (int)numericUpDown2.Value;
            _maxAge = (int)numericUpDown3.Value;

            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;
            if (regionId <= 0)
                regionId = 10;

            // Сохраняем настройки коэффициентов
            SaveRegionCoefficientSettings(regionId);

            CalculateAndPaintCoefficent(regionId);

            // Создаем прогноз численности постоянного населения по годам
            CreatePopulationForecastByYeart(regionId);

        }

        private class YearControl
        {
            public Label YearLabel { get; set; }
            public NumericUpDown YearNumericUpDown { get; set; }

            public YearControl(int year, Point location)
            {
                YearLabel = new Label
                {
                    Text = year.ToString(),
                    Location = location,
                    AutoSize = true
                };

                YearNumericUpDown = new NumericUpDown
                {
                    Location = new Point(location.X, location.Y + 20),
                    Width = 60,
                    DecimalPlaces = 2,
                    Increment = 0.01m,
                    Minimum = 0,
                    Maximum = 1,
                    Value = 1,
                };
            }
        }

        private void CreatePopulationForecastByYeart(int regionId)
        {
            forecastinForOneYear.Series.Clear();
            forecastinForOneYear.ChartAreas[0].AxisX.Title = "Возраст";
            forecastinForOneYear.ChartAreas[0].AxisY.Title = "Численность населения в тысячах";
            forecastinForOneYear.ChartAreas[0].AxisX.Minimum = 0;
            var xValues = new List<double>();
            var yValues = new List<double>();

            // Получаем данные о населении
            var populationData = new List<RegionStatisticsDto>();
            populationData = CalculationStorage.Instance.GetPermanentPopulationForecastDataValues();
            if (!populationData.Any())
                populationData = _populationRepository.GetPopulationInRegion(regionId);

            var maleCoefficients = CalculationStorage.Instance.GetLifeExpectancyDataMale();
            var femaleCoefficients = CalculationStorage.Instance.GetLifeExpectancyDataFemale();
            // Получаем прогнозы рождаемости для 0 лет
            var birthRates = _populationRepository.GetBirthRateEntitiesByRegionNumber(regionId);
            var visualizationYears = new List<int> { 2025, 2030, 2035, 2040, 2045 };
            var lastYearData = populationData.Where(dto => dto.Year == 2024).ToList();
            (xValues, yValues) = GetValuesForChart(lastYearData);
            var seriesLastYear = _painter.PainLinearGraph($"Факт {2024}", xValues, yValues);
            forecastinForOneYear.Series.Add(seriesLastYear);
            var forecastByYears = new Dictionary<int, List<RegionStatisticsDto>>
            {
                { 2024, lastYearData }
            };
            for (int year = 2025; year < 2046; year++)
            {
                var dtos = CreateEmptyDtos(year);
                dtos[0].SummaryByYearSmoothed = birthRates.FirstOrDefault(x => x.Year == year).BirthRate * 0.51;
                dtos[1].SummaryByYearSmoothed = birthRates.FirstOrDefault(x => x.Year == year).BirthRate * 0.49;
                for (int index = 2; index < dtos.Count - 1; index += 2)
                {
                    var age = index / 2;
                    var addValue = age > 2 ? -1 : 0;
                    var maleCoefficent = maleCoefficients.FirstOrDefault(x => x.Age == age + addValue)?.Coefficent;
                    var femaleCoefficent = femaleCoefficients.FirstOrDefault(x => x.Age == age + addValue)?.Coefficent;
                    var maleCount = forecastByYears[year - 1].FirstOrDefault(x => x.Age == age + addValue && x.Gender == Gender.Male)?.SummaryByYearSmoothed;
                    var femaleCount = forecastByYears[year - 1].FirstOrDefault(x => x.Age == age + addValue && x.Gender == Gender.Female)?.SummaryByYearSmoothed;
                    if (maleCoefficent == null || femaleCoefficent == null || maleCount == null || femaleCount == null)
                        continue;

                    dtos[index].SummaryByYearSmoothed = maleCount.Value * maleCoefficent.Value;
                    dtos[index + 1].SummaryByYearSmoothed = femaleCount.Value * femaleCoefficent.Value;
                }
                forecastByYears.Add(year, dtos);
            }

            var maxYValue = 0.0;
            foreach (var chartYear in visualizationYears)
            {
                var chartData = forecastByYears[chartYear];
                (xValues, yValues) = GetValuesForChart(chartData);
                maxYValue = Math.Max(maxYValue, yValues.Max());
                var series = _painter.PainLinearGraph($"{chartYear}", xValues, yValues);
                forecastinForOneYear.Series.Add(series);
            }

            forecastinForOneYear.ChartAreas[0].AxisY.Maximum = maxYValue * 1.3;
            CalculationStorage.Instance.PermanentPopulationForecast = forecastByYears;
            FillForecastInOneAgeChart();
        }

        private (List<double>, List<double>) SelectByGender(List<RegionStatisticsDto> dtos)
        {
            switch ((GenderComboBox)genderComboBox.SelectedIndex)
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

        private List<RegionStatisticsDto> CreateEmptyDtos(int year)
        {
            var currentAge = 0;
            var result = new List<RegionStatisticsDto>();
            for (int i = 0; i < _maxAge; i++)
            {
                result.Add(new RegionStatisticsDto
                {
                    Gender = Gender.Male,
                    Age = currentAge,
                    Year = year
                });
                result.Add(new RegionStatisticsDto
                {
                    Gender = Gender.Female,
                    Age = currentAge,
                    Year = year
                });
                currentAge++;
            }

            return result;
        }

        private (List<double>, List<double>) GetValuesForChart(List<RegionStatisticsDto> dtos)
        {
            var xValues = new List<double>();
            var yValues = new List<double>();

            foreach (var age in dtos.Select(dto => dto.Age).Distinct())
            {
                xValues.Add(age);
                yValues.Add(dtos.Where(dto => dto.Age == age).Sum(dto => dto.SummaryByYearSmoothed));
            }

            return (xValues, yValues);
        }

        /// <summary>
        /// Заполняет график прогноза численности населения по возрастным группам
        /// </summary>
        /// <param name="regionId">Идентификатор региона</param>
        private void FillForecastInOneAgeChart()
        {
            forecastionInOneAge.Series.Clear();
            forecastionInOneAge.ChartAreas[0].AxisX.Title = "Год";
            forecastionInOneAge.ChartAreas[0].AxisY.Title = "Численность населения в тысячах";
            var ageGroupps = new Dictionary<int, List<double>>();
            var years = new List<double>();
            for (int year = 2024; year < 2045; year++)
                years.Add(year);

            var forecastValues = CalculationStorage.Instance.PermanentPopulationForecast;
            foreach (var currentValues in forecastValues.Values)
            {
                for (int age = 10; age < 80; age += 10)
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
                var series = _painter.PainLinearGraph($"Возраст: {values.Key}", years, values.Value);
                forecastionInOneAge.Series.Add(series);
                seriesDataList.Add(new ChartDataService.SeriesData
                {
                    SeriesName = "Возраст: {values.Key}",
                    YValues = values.Value,
                    XValues = years
                });
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки перед переходом на следующую форму
            SaveSettings();

            FormRouting.NextForm(1, this);
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null && radioButton.Checked)
            {
                switch (radioButton.Name)
                {
                    case "NoTrim":
                        _maxCoefficentValue = decimal.MaxValue;
                        SetActiveToLamdaNumericUpDown(false);
                        break;
                    case "TrimToOne":
                        _maxCoefficentValue = 1;
                        SetActiveToLamdaNumericUpDown(false);
                        break;
                    default:
                        _maxCoefficentValue = 1 + numericUpDown1.Value / (decimal)100; 
                        SetActiveToLamdaNumericUpDown(true);
                        break;
                }
                CalculateCoefficents();
            }
        }

        private void SetActiveToLamdaNumericUpDown(bool active)
        {
            if (active)
            {
                numericUpDown1.Enabled = true;
                return;
            }
            numericUpDown1.Enabled = false;
        }

        private void YearNumerics_ValueChanged(object sender, EventArgs e)
        {
            CalculateCoefficents();
        }

        private void AgeNumerics_ValueChanged(object sender, EventArgs e)
        {
            CalculateCoefficents();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            TrimToDelta.Text = $"Обрезать до {100 + numericUpDown1.Value}%";
            _maxCoefficentValue = 1 + numericUpDown1.Value / (decimal)100;
            if (!TrimToDelta.Checked)
            {
                TrimToDelta.Checked = true;
                TrimToOne.Checked = false;
                NoTrim.Checked = false;
            }
            CalculateCoefficents();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Выберите директорию для сохранения прогноза в формате Excel";
            var regionName = RegionRepository.GetRegionNameById(CalculationStorage.Instance.CurrentRegion);

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _excelParser.FillForecastFile(folderBrowserDialog1.SelectedPath, regionName, $"Прогноз численности постоянного населения({regionName})", CalculationStorage.Instance.PermanentPopulationForecast);
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки перед переходом на следующую форму
            SaveSettings();

            FormRouting.PreviousForm(1, this);
        }
    }
}
