using System.Data;
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

        public ForecastionForm()
        {
            InitializeComponent();
            Init();
            _populationRepository = new PopulationRepository();
            _painter = new LinearGraphPainter();
            _excelParser = new ExcelParser();
            forecastDictionary = new Dictionary<int, List<RegionStatisticsDto>>();
            // Получаем номер текущего региона из хранилища
            int regionId = CalculationStorage.Instance.CurrentRegion;

            // Если регион не выбран, используем регион по умолчанию (10)
            if (regionId <= 0)
                regionId = 10;

            // Обновляем заголовок формы, чтобы показать текущий регион
            this.Text = $"Прогноз занятого в экономике населения региона (ID: {regionId})";

            // Создаем прогноз численности занятого в экономике населения по годам
            CreateEmployedInEconomyForecast(regionId);
        }

        private void Init()
        {
            comboBox1.Items.Add("Все");
            comboBox1.Items.Add("Мужчины");
            comboBox1.Items.Add("Женщины");
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
            var inEconomyLevelDtos = CalculationStorage.Instance.GetInEconomyLevelSmoothed();
            if (!inEconomyLevelDtos.Any())
                inEconomyLevelDtos = CalculationStorage.Instance.GetInEconomyLevel();

            CalculateForecast(permanentPopulation, inEconomyLevelDtos);
        }

        private void CalculateForecast(Dictionary<int, List<RegionStatisticsDto>> permanentPopulation, List<RegionInEconomyLevelDto> inEconomyLevelDtos)
        {
            forecastDictionary = new Dictionary<int, List<RegionStatisticsDto>>();
            var xValues = new List<double>();
            var yValues = new List<double>();
            var step = (int)numericUpDown1.Value;
            maxY = 0.0;
            for (int year = 2024; year < (int)numericUpDown2.Value; year += step)
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
            maxY = Math.Max(yValues.Max(), maxY);
            forecast.ChartAreas[0].AxisY.Maximum = maxY * 1.3;
            forecast.ChartAreas[0].AxisX.Maximum = 70;
            forecast.ChartAreas[0].AxisX.Minimum = 12;
            forecast.Series.Add(_painter.PainLinearGraph($"Прогноз на {year}", xValues, yValues));
            forecastDictionary.Add(year, forecastValues);
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
            // Закрываем текущую форму
            this.Close();
            // Предыдущая форма (EconomyEmploedForm) будет показана автоматически
            // благодаря обработчику FormClosed в EconomyEmploedForm
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateEmployedInEconomyForecast(CalculationStorage.Instance.CurrentRegion);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            CreateEmployedInEconomyForecast(CalculationStorage.Instance.CurrentRegion);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CreateEmployedInEconomyForecast(CalculationStorage.Instance.CurrentRegion);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Выберите директорию для сохранения прогноза в формате Excel";

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _excelParser.CreateForecastFile(folderBrowserDialog1.SelectedPath, forecastDictionary);
            }
        }
    }
}
