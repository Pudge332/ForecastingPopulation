using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Infrastructure;

namespace ForecastingWorkingPopulation
{
    public partial class LifeExpectancyCoefficientForm : Form
    {
        private readonly IPopulationRepository _populationRepository;
        private readonly LinearGraphPainter _painter;

        private int _minAge = 0;
        private int _maxAge = 80;

        private Dictionary<int, YearControl> _yearControls = new Dictionary<int, YearControl>();

        public LifeExpectancyCoefficientForm()
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

            // Загружаем настройки коэффициентов для текущего региона

            CalculateAndPaintCoefficent(regionId);
            LoadRegionCoefficientSettings(regionId);
            CalculateAndPaintCoefficent(regionId);
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

        private void CalculateAndPaintCoefficent(int regionNumber)
        {
            chart1.Series.Clear();
            var chartArea = chart1.ChartAreas[0];
            chartArea.AxisY.Maximum = 1.5;
            var ages = new List<double>();
            var coefficentDtos = new List<RegionCoefficentDto>();
            var dtos = new List<RegionStatisticsDto>(CalculationStorage.Instance.GetRegionStatisticsValues());
            var years = dtos.Select(dto => dto.Year).Distinct().OrderBy(x => x);

            // Обновляем заголовок формы, чтобы показать текущий регион
            this.Text = $"Коэффициенты продолжительности жизни региона (ID: {regionNumber})";

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
            CalculationStorage.Instance.StoreLifeExpectancyCalculation(grouppedDtos);
            CalculationStorage.Instance.StoreAvailableYears(years.ToList());
            CreateYearControls();
        }

        private void CreateYearControls()
        {
            var startX = 10;
            var startY = chart1.Bottom + 10;

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

            return 1 + Convert.ToDouble(numericUpDown1.Value / 100);
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
                    Coefficent = coefficent
                });
            }

            return coefficents.Where(coefficent => coefficent.Age >= _minAge && coefficent.Age <= _maxAge).ToList();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

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
    }
}
