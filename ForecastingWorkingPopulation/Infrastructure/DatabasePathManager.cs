using System;
using System.IO;

namespace ForecastingWorkingPopulation.Infrastructure
{
    /// <summary>
    /// Класс для управления путями к файлам базы данных
    /// </summary>
    public static class DatabasePathManager
    {
        /// <summary>
        /// Получает абсолютный путь к файлу базы данных
        /// </summary>
        /// <param name="fileName">Имя файла базы данных</param>
        /// <returns>Абсолютный путь к файлу базы данных</returns>
        public static string GetDatabasePath(string fileName = "Population.db")
        {
            // Получаем путь к директории, где расположен исполняемый файл приложения
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);

            // Формируем полный путь к файлу базы данных
            string dbPath = Path.Combine(appPath, fileName);

            // Проверяем существование файла базы данных
            if (!File.Exists(dbPath))
            {
                // Логируем информацию об отсутствии файла базы данных
                Console.WriteLine($"Файл базы данных не найден по пути: {dbPath}");

                // Можно добавить дополнительную логику для создания пустой базы данных или поиска в альтернативных местах
            }

            return dbPath;
        }
    }
}
