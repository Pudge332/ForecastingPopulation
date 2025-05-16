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
using System.Windows.Forms;

namespace ForecastingWorkingPopulation
{
    public partial class ParseExcelForm : Form
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

                // Открываем форму выбора действия
                var actionForm = new RegionActionForm(regionId, regionName);
                if (actionForm.ShowDialog() == DialogResult.OK)
                {
                    // Если пользователь выбрал "Выбрать файл для региона"
                    SelectFileForRegion(regionId);
                }
            }
        }

        // Выбор файла для региона
        private void SelectFileForRegion(int regionId)
        {
            openFileDialog1.Multiselect = false;
            // Настройка диалога выбора файла
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx";
            openFileDialog1.Title = "Выберите файл Excel";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var dtos = new List<RegionStatisticsDto>();

                string filePath = openFileDialog1.FileName;

                dtos = ParseRegionStatistic(filePath);
                SaveDataToDatabase(regionId, "Население в экономике", dtos, true);

            }
        }

        private void SaveDataToDatabase(int regionId, string dataType, List<RegionStatisticsDto> dtos, bool withMessage)
        {
            if (dataType == "Население в экономике")
                _populationRepository.SaveEmployedEconomyEntyties(regionId, dtos);
            else
                _populationRepository.SavePermanentPopulationEntyties(regionId, dtos);

            if (withMessage)
                NotificationForm.ShowSuccess($"Данные для региона с ID {regionId} ({dataType}) успешно сохранены. Количество строк {dtos.Count}");
        }

        private void SaveDataToDatabase(List<BirthRateEntity> dtos)
        {
            _populationRepository.SaveBirthRateEntyties(dtos);

            NotificationForm.ShowSuccess($"Загружено {dtos.Count} новых прогнозов рождаемости");
        }

        private List<RegionStatisticsDto> ParseRegionStatistic(string filePath)
        {
            return _excelParser.Parse(filePath, 5, new List<int> { 2019, 2020, 2021, 2022, 2023 });
        }

        private void ParseBirthRateBiluten(string filePath)
        {
            var birthRateDtos = _excelParser.ParseBirthRateBiluten(filePath);
            SaveDataToDatabase(birthRateDtos);
        }

        private (int, int) ParseBiluten(string filePath)
        {
            var rowsCount = 0;
            var regionCount = 0;
            var bilutenType = BilutenType.Old;
            var bilutenWorksheets = _excelParser.GetBulitenWorksheets(filePath, ref bilutenType);
            var columnOffset = bilutenType.GetCustomAttribute<BilutenTypeAttribute>().AgeColumnOffset;
            regionCount = bilutenWorksheets.Count;

            // Настраиваем ProgressBar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Visible = true;

            for (int i = 0; i < bilutenWorksheets.Count; i++)
            {
                var currentRegion = bilutenWorksheets[i];
                var dtos = _excelParser.ParseBiluten(filePath, currentRegion.WorkSheetName, columnOffset);
                rowsCount += dtos.Count;

                SaveDataToDatabase(currentRegion.Number, "Постоянное население", dtos, false);

                // Обновляем прогресс
                UpdateProgressBar((i + 1) * 100 / bilutenWorksheets.Count);
            }

            // Скрываем ProgressBar после завершения
            progressBar1.Visible = false;

            return (regionCount, rowsCount);
        }

        // Метод для обновления ProgressBar
        private void UpdateProgressBar(int percentComplete)
        {
            if (percentComplete < progressBar1.Minimum)
                percentComplete = progressBar1.Minimum;
            if (percentComplete > progressBar1.Maximum)
                percentComplete = progressBar1.Maximum;

            progressBar1.Value = percentComplete;
            progressBar1.Update();
            Application.DoEvents(); // Позволяет обновить интерфейс во время выполнения операции
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            var rowsCount = 0;
            var regionCount = 0;

            // Настройка диалога выбора файла
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx";
            openFileDialog1.Title = "Выберите файл Бюллетени в формате Excel";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePaths = openFileDialog1.FileNames;

                foreach (var filePath in filePaths)
                {
                    (regionCount, rowsCount) = ParseBiluten(filePath);

                    if (regionCount > 0)
                        NotificationForm.ShowSuccess($"По {regionCount} регионам успешно загруженно {rowsCount} строк из файла {GetFileName(filePath)}");
                }
            }
        }

        private string GetFileName(string path)
        {
            var pathShards = path.Split('\\');
            return pathShards.Last();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Настройка диалога выбора файла
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx";
            openFileDialog1.Title = "Выберите файл Бюллетени в формате Excel";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;
                ParseBirthRateBiluten(filePath);
            }
        }

        private void massImportFiles_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            progressBar1.Visible = true;
            var regionCount = 0;
            var rowsCount = 0;
            var regions = RegionRepository.GetRegions();
            // Настройка диалога выбора файла
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx";
            openFileDialog1.Title = "Выберите файл опросов в формате Excel";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePaths = openFileDialog1.FileNames;

                foreach(var path in filePaths)
                {
                    var fileName = GetFileName(path)?.Replace(".xlsx", "");
                    if (fileName is null)
                        continue;

                    var region = regions.FirstOrDefault(x => x.Name.ToLowerInvariant() == fileName.ToLowerInvariant());
                    if (region is null)
                        continue;
                    var dtos = ParseRegionStatistic(path);
                    SaveDataToDatabase(region.Number, "Население в экономике", dtos, false);
                    regionCount++;
                    rowsCount += dtos?.Count() ?? 0;
                    UpdateProgressBar(regionCount * 100 / filePaths.Count());
                }

                progressBar1.Value = 0;
                progressBar1.Visible = false;
                if (regionCount > 0)
                    NotificationForm.ShowSuccess($"По {regionCount} регионам успешно загруженно {rowsCount} строк");
            }
        }
    }
}
