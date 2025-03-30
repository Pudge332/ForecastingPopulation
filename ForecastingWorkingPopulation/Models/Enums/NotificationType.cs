using System;
using System.Drawing;

namespace ForecastingWorkingPopulation.Models.Enums
{
    /// <summary>
    /// Типы уведомлений для NotificationForm
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Уведомление об успешном выполнении операции
        /// </summary>
        Success,

        /// <summary>
        /// Уведомление об ошибке
        /// </summary>
        Error,

        /// <summary>
        /// Информационное уведомление
        /// </summary>
        Info,

        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning
    }

    /// <summary>
    /// Расширения для работы с типами уведомлений
    /// </summary>
    public static class NotificationTypeExtensions
    {
        /// <summary>
        /// Получить цвет для типа уведомления
        /// </summary>
        /// <param name="type">Тип уведомления</param>
        /// <returns>Цвет для отображения</returns>
        public static Color GetColor(this NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Success:
                    return Color.FromArgb(76, 175, 80);
                case NotificationType.Error:
                    return Color.FromArgb(244, 67, 54);
                case NotificationType.Info:
                    return Color.FromArgb(33, 150, 243);
                case NotificationType.Warning:
                    return Color.FromArgb(255, 152, 0);
                default:
                    return Color.Gray;
            }
        }

        /// <summary>
        /// Получить заголовок для типа уведомления
        /// </summary>
        /// <param name="type">Тип уведомления</param>
        /// <returns>Текст заголовка</returns>
        public static string GetTitle(this NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Success:
                    return "Успех";
                case NotificationType.Error:
                    return "Ошибка";
                case NotificationType.Info:
                    return "Информация";
                case NotificationType.Warning:
                    return "Предупреждение";
                default:
                    return "Уведомление";
            }
        }
    }
}
