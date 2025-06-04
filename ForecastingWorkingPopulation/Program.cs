using ForecastingWorkingPopulation.Contracts.Interfaces;
using ForecastingWorkingPopulation.Database.Context;
using ForecastingWorkingPopulation.Database.Models;
using ForecastingWorkingPopulation.Database.Repositories;
using ForecastingWorkingPopulation.Infrastructure;
using ForecastingWorkingPopulation.Infrastructure.Excel;
using ForecastingWorkingPopulation.Models.Dto;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ForecastingWorkingPopulation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Проверяем доступность базы данных перед запуском приложения
            CheckDatabaseAvailability();

            SetDependecies();
            Application.Run(new ParseExcelForm());
        }

        /// <summary>
        /// Проверяет доступность базы данных и выполняет необходимую инициализацию
        /// </summary>
        private static void CheckDatabaseAvailability()
        {
            try
            {
                // Получаем путь к файлу базы данных
                string dbPath = DatabasePathManager.GetDatabasePath();

                // Проверяем существование файла базы данных
                if (!File.Exists(dbPath))
                {
                    MessageBox.Show(
                        $"Файл базы данных не найден по пути: {dbPath}\n\nПриложение может работать некорректно без файла базы данных.",
                        "Предупреждение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                // Проверяем доступность базы данных, пытаясь создать контекст
                using (var context = new PopulationContext())
                {
                    // Проверяем соединение с базой данных
                    context.Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при инициализации базы данных: {ex.Message}\n\nПриложение может работать некорректно.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void SetDependecies()
        {
            var services = new ServiceCollection();
            services.AddScoped<IExcelParser, ExcelParser>();
            services.AddScoped<IPopulationRepository, PopulationRepository>();

            var serviceProvider = services.BuildServiceProvider();
        }
    }
}
