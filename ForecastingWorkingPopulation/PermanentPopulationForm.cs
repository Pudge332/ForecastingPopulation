using System.Data;
using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Infrastructure;
using ForecastingWorkingPopulation.Models.Enums;
using System.Windows.Forms.DataVisualization.Charting;

namespace ForecastingWorkingPopulation
{
    public partial class PermanentPopulationForm: Form
    {
        private readonly IPopulationRepository _populationRepository;
        private readonly LinearGraphPainter _painter;
        private bool _isSetting = false;
        private int _windowSize = 5; // Значение по умолчанию для размера окна сглаживания

        private int _minAge = 0;
        private int _maxAge = 80;

        private Dictionary<int, YearControl> _yearControls = new Dictionary<int, YearControl>();

        public PermanentPopulationForm()
        {
            InitializeComponent();
            _populationRepository = new PopulationRepository();
            _painter = new LinearGraphPainter();
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

            // Заполняем график прогноза численности населения по возрастным группам
            FillForecastInOneAgeChart(regionId);

            // Создаем прогноз численности постоянного населения по годам
            CreatePopulationForecastByYear(regionId);
        }

        private void Init()
        {
            label1.Text = "Отключить обрезку коэффицентов > 1";
            label2.Text = "100% + *% = ";
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
                if ((int)smoothComboValue > 0)
                {
                    yValues = MovingAverageSmoothing(currentGroup, windowSize: _windowSize, (int)smoothComboValue);
                    var smoothedDtos = UpdateSmoothedValues(group.ToList(), yValues);
                    CalculationStorage.Instance.StorePermanentPopulationStatisticsSmoothed(group.Key, smoothedDtos);
                }
                else
                {
                    CalculationStorage.Instance.StorePermanentPopulationRegionStatistics(group.Key, group.ToList());
                }

                var series = _painter.PainLinearGraph($"Год {group.Key}", xValues, yValues);
                currentChart.Series.Add(series);
                CalculationStorage.Instance.StoreEconomyEmploedRegionStatistics(group.Key, currentGroup.ToList());

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

            // Устанавливаем максимальное значение по оси Y
            SetChartYAxisMaximum(currentChart, seriesDataList, "PermanentPopulationMaxY");
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
            double finalMaxY = Math.Max(newMaxY, savedMaxY);

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

        private List<RegionStatisticsDto> UpdateSmoothedValues(List<RegionStatisticsDto> dtos, List<double> ySmoothed)
        {
            for (int i = dtos.Count - ySmoothed.Count; i < ySmoothed.Count; i++)
                dtos[i].SummaryByYearSmoothed = ySmoothed[i];

            return dtos;
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
                    _yearControls[year] = yearControl;

                    startX += 70; // Spacing between year controls
                }
            }
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
            if (checkBox1.Checked)
                return double.PositiveInfinity;

            return 1 + Convert.ToDouble(numericUpDown1.Value);
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
            if(!weights.Any())
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

                //Сохраняется текущий год, из пары 2019 и 2020 буде сохранен 2019
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
            checkBox1.Checked = settings.DisableCoefficientCutoff;
            numericUpDown2.Value = settings.MinAge;
            numericUpDown3.Value = settings.MaxAge;

            // Устанавливаем коэффициенты для годов
            if (_yearControls.ContainsKey(2019) && settings.Coefficient2019 >= 0)
                _yearControls[2019].YearNumericUpDown.Value = (decimal)settings.Coefficient2019;
            if (_yearControls.ContainsKey(2020) && settings.Coefficient2020 >= 0)
                _yearControls[2020].YearNumericUpDown.Value = (decimal)settings.Coefficient2020;
            if (_yearControls.ContainsKey(2021) && settings.Coefficient2021 >= 0)
                _yearControls[2021].YearNumericUpDown.Value = (decimal)settings.Coefficient2021;
            if (_yearControls.ContainsKey(2022) && settings.Coefficient2022 >= 0)
                _yearControls[2022].YearNumericUpDown.Value = (decimal)settings.Coefficient2022;
            if (_yearControls.ContainsKey(2023) && settings.Coefficient2023 >= 0)
                _yearControls[2023].YearNumericUpDown.Value = (decimal)settings.Coefficient2023;
            if (_yearControls.ContainsKey(2024) && settings.Coefficient2024 >= 0)
                _yearControls[2024].YearNumericUpDown.Value = (decimal)settings.Coefficient2024;
        }

        private void SaveRegionCoefficientSettings(int regionNumber)
        {
            var settings = new ForecastingWorkingPopulation.Database.Models.RegionCoefficientSettingsEntity
            {
                RegionNumber = regionNumber,
                MinAge = (int)numericUpDown2.Value,
                MaxAge = (int)numericUpDown3.Value,
                CoefficientLimit = numericUpDown1.Value,
                DisableCoefficientCutoff = checkBox1.Checked
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

        private void button1_Click(object sender, EventArgs e)
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

            // Обновляем график прогноза численности населения
            FillForecastInOneAgeChart(regionId);

            // Создаем прогноз численности постоянного населения по годам
            CreatePopulationForecastByYear(regionId);
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
                    Increment = 0.1m,
                    Minimum = 0,
                    Maximum = 10,
                    Value = 1
                };
            }
        }

        /// <summary>
        /// Создает прогноз численности постоянного населения по годам с 2025 по 2045 с шагом 5 лет
        /// </summary>
        /// <param name="regionId">Идентификатор региона</param>
        private void CreatePopulationForecastByYear(int regionId)
        {
            // Очищаем график перед добавлением новых данных
            forecastinForOneYear.Series.Clear();
            forecastinForOneYear.ChartAreas[0].AxisX.Title = "Возраст";
            forecastinForOneYear.ChartAreas[0].AxisY.Title = "Численность населения в тысячах";

            // Получаем данные о населении
            var populationData = CalculationStorage.Instance.GetPermanentPopulationRegionStatisticsValues();
            if (!populationData.Any())
                populationData = _populationRepository.GetPopulationInRegion(regionId);

            // Получаем коэффициенты для прогнозирования
            var maleCoefficients = CalculationStorage.Instance.GetLifeExpectancyDataMale();
            var femaleCoefficients = CalculationStorage.Instance.GetLifeExpectancyDataFemale();

            // Получаем прогнозы рождаемости для 0 лет
            var birthRates = _populationRepository.GetBirthRateEntitiesByRegionNumber(regionId);

            // Определяем годы для визуализации с шагом 5 лет
            var visualizationYears = new List<int> { 2025, 2030, 2035, 2040, 2045 };

            // Получаем последний известный год данных
            int lastKnownYear = populationData.Max(p => p.Year);

            // Создаем словари для хранения прогнозных данных по годам и полу
            var maleForecastByYear = new Dictionary<int, Dictionary<int, double>>(); // год -> возраст -> численность
            var femaleForecastByYear = new Dictionary<int, Dictionary<int, double>>(); // год -> возраст -> численность

            // Инициализируем начальные значения из последнего известного года
            maleForecastByYear[lastKnownYear] = new Dictionary<int, double>();
            femaleForecastByYear[lastKnownYear] = new Dictionary<int, double>();

            // Заполняем начальные данные
            foreach (var dto in populationData.Where(p => p.Year == lastKnownYear))
            {
                if (dto.Gender == Gender.Male)
                    maleForecastByYear[lastKnownYear][dto.Age] = dto.SummaryByYear;
                else
                    femaleForecastByYear[lastKnownYear][dto.Age] = dto.SummaryByYear;
            }

            // Рассчитываем прогноз для каждого года от lastKnownYear+1 до 2045
            for (int year = lastKnownYear + 1; year <= 2045; year++)
            {
                maleForecastByYear[year] = new Dictionary<int, double>();
                femaleForecastByYear[year] = new Dictionary<int, double>();

                // Прогнозируем для каждого возраста
                for (int age = 0; age <= 100; age++)
                {
                    // Для возраста 0 используем данные о рождаемости
                    if (age == 0)
                    {
                        var birthRate = birthRates?.FirstOrDefault(b => b.Year == year)?.BirthRate ??
                                      (birthRates?.OrderByDescending(b => b.Year).FirstOrDefault()?.BirthRate ?? 0);

                        // Распределяем рождаемость между полами (примерно 51% мальчиков, 49% девочек)
                        maleForecastByYear[year][age] = birthRate * 0.51;
                        femaleForecastByYear[year][age] = birthRate * 0.49;
                    }
                    else
                    {
                        // Для остальных возрастов используем метод передвижки с коэффициентами
                        // Находим подходящие коэффициенты для предыдущего возраста
                        var maleCoef = maleCoefficients
                            .Where(c => c.Age == age - 1)
                            .Select(c => c.Coefficent)
                            .DefaultIfEmpty(1.0)
                            .Average();

                        var femaleCoef = femaleCoefficients
                            .Where(c => c.Age == age - 1)
                            .Select(c => c.Coefficent)
                            .DefaultIfEmpty(1.0)
                            .Average();

                        // Применяем коэффициенты к предыдущему году и возрасту
                        // Используем метод передвижек - берем население предыдущего года и возраста и умножаем на коэффициент дожития
                        double prevMaleValue = 0;
                        double prevFemaleValue = 0;

                        // Получаем значения для предыдущего года и возраста
                        if (maleForecastByYear.TryGetValue(year - 1, out var maleAgeDict))
                        {
                            maleAgeDict.TryGetValue(age - 1, out prevMaleValue);
                        }

                        if (femaleForecastByYear.TryGetValue(year - 1, out var femaleAgeDict))
                        {
                            femaleAgeDict.TryGetValue(age - 1, out prevFemaleValue);
                        }

                        // Применяем коэффициенты к предыдущему году и возрасту
                        maleForecastByYear[year][age] = prevMaleValue * maleCoef;
                        femaleForecastByYear[year][age] = prevFemaleValue * femaleCoef;
                    }
                }
            }

            // Добавляем фактические данные за последний известный год
            var lastYearData = populationData.Where(p => p.Year == lastKnownYear);
            var xValuesLastYear = new List<double>();
            var yValuesLastYear = new List<double>();

            // Группируем данные по возрасту и суммируем по полу
            foreach (var ageGroup in lastYearData.GroupBy(p => p.Age).OrderBy(g => g.Key))
            {
                if (ageGroup.Key >= _minAge && ageGroup.Key <= _maxAge)
                {
                    xValuesLastYear.Add(ageGroup.Key);
                    yValuesLastYear.Add(ageGroup.Sum(p => p.SummaryByYear));
                }
            }

            // Добавляем серию для последнего известного года
            var seriesLastYear = _painter.PainLinearGraph($"Факт {lastKnownYear}", xValuesLastYear, yValuesLastYear);
            seriesLastYear.Color = System.Drawing.Color.Black;
            seriesLastYear.BorderWidth = 3;
            forecastinForOneYear.Series.Add(seriesLastYear);

            // Добавляем прогнозные данные только для годов с шагом в 5 лет (для визуализации)
            foreach (var year in visualizationYears.Where(y => y > lastKnownYear))
            {
                var xValues = new List<double>();
                var yValues = new List<double>();

                // Для каждого возраста суммируем данные по мужчинам и женщинам
                for (int age = _minAge; age <= _maxAge; age++)
                {
                    xValues.Add(age);
                    double maleValue = maleForecastByYear[year].GetValueOrDefault(age, 0);
                    double femaleValue = femaleForecastByYear[year].GetValueOrDefault(age, 0);
                    yValues.Add(maleValue + femaleValue);
                }

                // Создаем серию для текущего прогнозного года
                var series = _painter.PainLinearGraph($"Прогноз {year}", xValues, yValues);
                forecastinForOneYear.Series.Add(series);
            }

            // Устанавливаем максимальное значение по оси Y
            var seriesDataList = PermanentPopulation.Series
                .Select(s => new ChartDataService.SeriesData {
                    SeriesName = s.Name,
                    XValues = s.Points.Select(p => p.XValue).ToList(),
                    YValues = s.Points.Select(p => p.YValues[0]).ToList()
                })
                .ToList();

            SetChartYAxisMaximum(PermanentPopulation, seriesDataList, "PermanentPopulationMaxY");
        }

        /// <summary>
        /// Заполняет график прогноза численности населения по возрастным группам
        /// </summary>
        /// <param name="regionId">Идентификатор региона</param>
        private void FillForecastInOneAgeChart(int regionId)
        {
            forecastionInOneAge.Series.Clear();
            forecastionInOneAge.ChartAreas[0].AxisX.Title = "Год";
            forecastionInOneAge.ChartAreas[0].AxisY.Title = "Численность населения в тысячах";

            // Получаем данные о населении
            var populationData = CalculationStorage.Instance.GetPermanentPopulationRegionStatisticsValues();
            if (!populationData.Any())
                populationData = _populationRepository.GetPopulationInRegion(regionId);

            // Получаем коэффициенты для прогнозирования
            var maleCoefficients = CalculationStorage.Instance.GetLifeExpectancyDataMale();
            var femaleCoefficients = CalculationStorage.Instance.GetLifeExpectancyDataFemale();

            // Получаем прогнозы рождаемости для 0 лет
            var birthRates = _populationRepository.GetBirthRateEntitiesByRegionNumber(regionId);

            // Определяем возрастные группы с шагом 10 лет, начиная с 10
            var ageGroups = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80 };

            // Определяем текущий год и последний год прогноза
            int currentYear = 2025;
            int lastYear = 2045;

            // Для каждой возрастной группы создаем серию данных
            foreach (var age in ageGroups)
            {
                var xValues = new List<double>(); // Годы
                var yValues = new List<double>(); // Численность населения

                // Получаем текущие значения для возрастной группы (до 2025 года)
                var historicalYears = populationData
                    .Where(p => p.Age == age)
                    .GroupBy(p => p.Year)
                    .OrderBy(g => g.Key);

                foreach (var yearGroup in historicalYears)
                {
                    if (yearGroup.Key <= currentYear)
                    {
                        xValues.Add(yearGroup.Key);

                        // Суммируем данные по мужчинам и женщинам
                        double totalPopulation = yearGroup
                            .Sum(p => p.SummaryByYear);

                        yValues.Add(totalPopulation);
                    }
                }

                // Прогнозируем численность населения для будущих лет (после 2025)
                // Берем последние известные данные как базу для прогноза
                var lastKnownYear = historicalYears.LastOrDefault()?.Key ?? currentYear;
                var lastKnownData = populationData
                    .Where(p => p.Year == lastKnownYear && p.Age == age)
                    .ToList();

                if (lastKnownData.Any())
                {
                    // Создаем словари для хранения прогнозных данных по годам и полу
                    var maleForecast = new Dictionary<int, double>();
                    var femaleForecast = new Dictionary<int, double>();

                    // Инициализируем начальные значения
                    maleForecast[lastKnownYear] = lastKnownData
                        .Where(p => p.Gender == Gender.Male)
                        .Sum(p => p.SummaryByYear);

                    femaleForecast[lastKnownYear] = lastKnownData
                        .Where(p => p.Gender == Gender.Female)
                        .Sum(p => p.SummaryByYear);

                    // Прогнозируем численность для каждого года
                    for (int year = lastKnownYear + 1; year <= lastYear; year++)
                    {
                        // Для возраста 0 используем данные о рождаемости
                        if (age == 0)
                        {
                            var birthRate = birthRates?.FirstOrDefault(b => b.Year == year)?.BirthRate ?? 0;
                            // Распределяем рождаемость между полами (примерно 51% мальчиков, 49% девочек)
                            maleForecast[year] = birthRate * 0.51;
                            femaleForecast[year] = birthRate * 0.49;
                        }
                        else
                        {
                            // Для остальных возрастов используем метод передвижки с коэффициентами
                            // Находим подходящие коэффициенты для текущего возраста
                            var maleCoef = maleCoefficients
                                .Where(c => c.Age == age - 1)
                                .Select(c => c.Coefficent)
                                .DefaultIfEmpty(1.0)
                                .Average();

                            var femaleCoef = femaleCoefficients
                                .Where(c => c.Age == age - 1)
                                .Select(c => c.Coefficent)
                                .DefaultIfEmpty(1.0)
                                .Average();

                            // Применяем коэффициенты к предыдущему году
                            maleForecast[year] = maleForecast.GetValueOrDefault(year - 1, 0) * maleCoef;
                            femaleForecast[year] = femaleForecast.GetValueOrDefault(year - 1, 0) * femaleCoef;
                        }

                        // Добавляем прогнозные данные в графики
                        xValues.Add(year);
                        yValues.Add(maleForecast[year] + femaleForecast[year]);
                    }
                }

                // Создаем серию для текущей возрастной группы
                var series = _painter.PainLinearGraph($"Возраст {age}", xValues, yValues);
                forecastionInOneAge.Series.Add(series);
            }

            // Устанавливаем максимальное значение по оси Y
            var seriesDataList = forecastionInOneAge.Series
                .Select(s => new ChartDataService.SeriesData {
                    SeriesName = s.Name,
                    XValues = s.Points.Select(p => p.XValue).ToList(),
                    YValues = s.Points.Select(p => p.YValues[0]).ToList()
                })
                .ToList();

            SetChartYAxisMaximum(forecastionInOneAge, seriesDataList, "PermanentPopulationMaxY");
        }
    }
}
