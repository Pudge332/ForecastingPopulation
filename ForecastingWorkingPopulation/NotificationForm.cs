using ForecastingWorkingPopulation.Models.Enums;

namespace ForecastingWorkingPopulation
{
    public partial class NotificationForm : Form
    {

        // Коллекция для отслеживания активных уведомлений
        private static List<NotificationForm> _activeNotifications = new List<NotificationForm>();

        // Таймер для автоматического закрытия
        private System.Windows.Forms.Timer _closeTimer;

        // Время отображения уведомления в миллисекундах
        private const int DefaultDisplayTime = 3000;

        // Конструктор формы
        public NotificationForm()
        {
            InitializeComponent();
            InitializeTimer();
        }

        // Инициализация таймера
        private void InitializeTimer()
        {
            _closeTimer = new System.Windows.Forms.Timer();
            _closeTimer.Interval = DefaultDisplayTime;
            _closeTimer.Tick += CloseTimer_Tick;
        }

        // Обработчик события таймера
        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            _closeTimer.Stop();
            CloseNotification();
        }

        // Метод для закрытия уведомления
        private void CloseNotification()
        {
            _activeNotifications.Remove(this);
            this.Close();
            this.Dispose();
        }

        // Статический метод для показа уведомления об успехе
        public static void ShowSuccess(string message)
        {
            Show(message, NotificationType.Success);
        }

        // Статический метод для показа уведомления об ошибке
        public static void ShowError(string message)
        {
            Show(message, NotificationType.Error);
        }

        // Статический метод для показа информационного уведомления
        public static void ShowInfo(string message)
        {
            Show(message, NotificationType.Info);
        }

        // Статический метод для показа предупреждения
        public static void ShowWarning(string message)
        {
            Show(message, NotificationType.Warning);
        }

        // Основной метод для показа уведомления
        private static void Show(string message, NotificationType type)
        {
            // Создаем новую форму уведомления
            NotificationForm notification = new NotificationForm();

            // Настраиваем внешний вид в зависимости от типа
            notification.ConfigureAppearance(type);

            // Устанавливаем текст уведомления
            notification.SetMessage(message);

            // Позиционируем уведомление
            notification.PositionNotification();

            // Добавляем в список активных уведомлений
            _activeNotifications.Add(notification);

            // Показываем уведомление
            notification.Show();

            // Запускаем таймер для автоматического закрытия
            notification._closeTimer.Start();
        }

        // Метод для настройки внешнего вида уведомления
        private void ConfigureAppearance(NotificationType type)
        {
            // Настраиваем размер формы
            this.Size = new Size(400, 100);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;

            // Настраиваем цвет и заголовок в зависимости от типа уведомления
            this.BackColor = type.GetColor();
            this.Text = type.GetTitle();
        }

        // Метод для установки текста уведомления
        private void SetMessage(string message)
        {
            // Создаем и настраиваем метку для текста
            Label messageLabel = new Label();
            messageLabel.Text = message;
            messageLabel.ForeColor = Color.White;
            messageLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            messageLabel.Dock = DockStyle.Fill;
            messageLabel.TextAlign = ContentAlignment.MiddleCenter;
            messageLabel.Padding = new Padding(10);

            // Добавляем метку на форму
            this.Controls.Add(messageLabel);
        }

        // Метод для позиционирования уведомления
        private void PositionNotification()
        {
            // Получаем размеры экрана
            Rectangle workingArea = Screen.GetWorkingArea(this);

            // Базовая позиция - правый нижний угол экрана
            int startX = workingArea.Right - this.Width - 20;
            int startY = workingArea.Bottom - this.Height - 20;

            // Смещаем вверх, если есть другие активные уведомления
            foreach (var notification in _activeNotifications)
            {
                startY -= notification.Height + 5;
            }

            // Устанавливаем позицию
            this.Location = new Point(startX, startY);
        }
    }
}
