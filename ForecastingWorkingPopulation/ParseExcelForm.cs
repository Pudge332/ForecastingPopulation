using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.Excel;
using ForecastingWorkingPopulation.Models.Attributes;
using ForecastingWorkingPopulation.Models.Dto;
using ForecastingWorkingPopulation.Models.Enums;
using ForecastingWorkingPopulation.Extensions;
using System.Data;
using System.Drawing;
using System.Reflection;

namespace ForecastingWorkingPopulation
{
    public partial class ParseExcelForm : Form
    {
        private readonly IExcelParser _excelParser;
        private readonly IPopulationRepository _populationRepository;
        private string _dataType = "Постоянное население";

        // Таблица для хранения списка регионов
        private DataTable _regionsTable;

        public ParseExcelForm()
        {
            InitializeComponent();
            _excelParser = new ExcelParser();
            _populationRepository = new PopulationRepository();
            Init();
        }

        // Загрузка формы
        private void Init()
        {
            checkBox1.Checked = false;
            label1.Text = _dataType;
            LoadRegions();
        }

        private void LoadRegions()
        {
            _regionsTable = new DataTable();
            _regionsTable.Columns.Add("Номер региона", typeof(int));
            _regionsTable.Columns.Add("Название региона", typeof(string));
            _regionsTable.Columns.Add("Дата последнего изменения", typeof(DateTime));

            var regions = _populationRepository.GetAllRegions();
            foreach (var region in regions)
                _regionsTable.Rows.Add(region.Number, region.Name, region.LastUpdateTime);

            dataGridView1.DataSource = _regionsTable;
        }

        // Обработчик двойного нажатия на строку в DataGridView
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Проверка, что нажатие было на строку, а не на заголовок
            {
                // Получение выбранного региона
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string regionName = selectedRow.Cells["Название региона"].Value.ToString();
                string lastUpdate = selectedRow.Cells["Дата последнего изменения"].Value.ToString();
                int regionId = (int)selectedRow.Cells["Номер региона"].Value;

                MessageBox.Show($"Выбран регион: {regionName}\nПоследнее обновление: {lastUpdate}\nНомер региона: {regionId}");

                // Выбор файла для региона
                SelectFileForRegion(regionId);
            }
        }

        // Выбор файла для региона
        private void SelectFileForRegion(int regionId)
        {
            // Настройка диалога выбора файла
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx";
            openFileDialog1.Title = "Выберите файл Excel";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var dtos = new List<RegionStatisticsDto>();

                string filePath = openFileDialog1.FileName;

                dtos = ParseRegionStatistic(filePath);
                SaveDataToDatabase(regionId, _dataType, dtos, true);

            }
        }

        private void SaveDataToDatabase(int regionId, string dataType, List<RegionStatisticsDto> dtos, bool withMessage)
        {
            if (dataType == "Население в экономике")
                _populationRepository.SaveEmployedEconomyEntyties(regionId, dtos);
            else
                _populationRepository.SavePermanentPopulationEntyties(regionId, dtos);

            if (withMessage)
                MessageBox.Show($"Данные для региона с ID {regionId} ({dataType}) успешно сохранены. Количество строк {dtos.Count}");
        }

        private List<RegionStatisticsDto> ParseRegionStatistic(string filePath)
        {
            return _excelParser.Parse(filePath, 5, new List<int> { 2019, 2020, 2021, 2022, 2023 });
        }

        private (int, int) ParseBiluten(string filePath)
        {
            var rowsCount = 0;
            var regionCount = 0;
            var bilutenType = BilutenType.Old;
            var bilutenWorksheets = _excelParser.GetBulitenWorksheets(filePath, ref bilutenType);
            var columnOffset = bilutenType.GetCustomAttribute<BilutenTypeAttribute>().AgeColumnOffset;
            regionCount = bilutenWorksheets.Count;

            foreach (var currentRegion in bilutenWorksheets)
            {
                var dtos = _excelParser.ParseBiluten(filePath, currentRegion.WorkSheetName, columnOffset);
                rowsCount += dtos.Count;

                SaveDataToDatabase(currentRegion.Number, "Постоянное население", dtos, false);
            }

            return (regionCount, rowsCount);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rowsCount = 0;
            var regionCount = 0;

            // Настройка диалога выбора файла
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx";
            openFileDialog1.Title = "Выберите файл Бюллетени в формате Excel";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                (regionCount, rowsCount) = ParseBiluten(filePath);
            }

            MessageBox.Show($"По {regionCount} регионам успешно загруженно {rowsCount} строк");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var boxChecked = "Население в экономике";
            var boxUnChecked = "Постоянное население";
            _dataType = checkBox1.Checked ? boxChecked : boxUnChecked;
            label1.Text = _dataType;
        }
    }
}
