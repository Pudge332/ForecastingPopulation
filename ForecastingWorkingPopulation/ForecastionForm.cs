using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure;
using ForecastingWorkingPopulation.Infrastructure.GraphPainting;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;
using System.Windows.Forms.DataVisualization.Charting;
using static OfficeOpenXml.ExcelErrorValue;

namespace ForecastingWorkingPopulation
{
    public partial class ForecastionForm: Form
    {
        private readonly IPopulationRepository _populationRepository;
        private readonly LinearGraphPainter _painter;

        public ForecastionForm()
        {
            InitializeComponent();
            _populationRepository = new PopulationRepository();
            _painter = new LinearGraphPainter();

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

        /// <summary>
        /// Создает прогноз численности занятого в экономике населения по годам
        /// </summary>
        /// <param name="regionId">Идентификатор региона</param>
        private void CreateEmployedInEconomyForecast(int regionId)
        {
            forecast.Series.Clear();
            forecast.ChartAreas[0].AxisX.Title = "Возраст";
            forecast.ChartAreas[0].AxisY.Title = "Численность занятого населения в тысячах";
            var xValues = new List<double>();
            var yValues = new List<double>();

            var permanentPopulation = CalculationStorage.Instance.PermanentPopulationForecast;
            var inEconomyLevelDtos = CalculationStorage.Instance.GetInEconomyLevel();

            var lastYear = permanentPopulation[2045];
            foreach(var age in lastYear.Select(dto => dto.Age).Distinct())
            {
                var dtosByYear = lastYear.Where(dto => dto.Age == age);
                foreach (var dto in dtosByYear)
                {
                    var inEconomyLevel = inEconomyLevelDtos.FirstOrDefault(x => x.Gender == dto.Gender && x.Age == dto.Age)?.Level;
                    if (inEconomyLevel == null)
                        inEconomyLevel = 0;

                    dto.SummaryByYearSmoothed = dto.SummaryByYearSmoothed * inEconomyLevel.Value;
                }
            }
            (xValues, yValues) = GetValuesForChart(lastYear);
            forecast.Series.Add(_painter.PainLinearGraph($"Все", xValues, yValues));
            (xValues, yValues) = GetValuesForChart(lastYear, Gender.Male);
            forecast.Series.Add(_painter.PainLinearGraph($"Мужчины", xValues, yValues));
            (xValues, yValues) = GetValuesForChart(lastYear, Gender.Female);
            forecast.Series.Add(_painter.PainLinearGraph($"Женщины", xValues, yValues));
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
    }
}
