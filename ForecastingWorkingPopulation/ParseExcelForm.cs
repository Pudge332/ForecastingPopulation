using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure.Excel;
using ForecastingWorkingPopulation.Models.Dto;
using System.Data;

namespace ForecastingWorkingPopulation
{
    public partial class ParseExcelForm: Form
    {
        private readonly IExcelParser _excelParser;
        private readonly IPopulationRepository _populationRepository;

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
                string filePath = openFileDialog1.FileName;

                // Определение типа данных (постоянное население или экономическое)
                string dataType = radioButton1.Checked ? "Постоянное население" : "Население в экономике";

                var dtos = _excelParser.Parse(filePath, 5, new List<int> { 2019, 2020, 2021, 2022, 2023 });
                // Сохранение данных в базу данных (заглушка)
                SaveDataToDatabase(regionId, dataType, dtos);
            }
        }

        private void SaveDataToDatabase(int regionId, string dataType, List<RegionStatisticsDto> dtos)
        {
            if (dataType == "Население в экономике") 
                _populationRepository.SaveEntityByRegion<EmployedEconomyPopulationInRegionEntity>(regionId, dtos);
            else
                _populationRepository.SaveEntityByRegion<PermanentPopulationInRegionEntity>(regionId, dtos);

            MessageBox.Show($"Данные для региона с ID {regionId} ({dataType}) успешно сохранены. Количество строк {dtos.Count}");
        }
    }
}
