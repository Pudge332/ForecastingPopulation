using System.Reflection;

namespace ForecastingWorkingPopulation.Infrastructure
{
    public static class FormRouting
    {
        public static void NextForm(int formNumber, Form currentForm)
        {
            // Сохраняем настройки текущей формы перед закрытием
            SaveFormSettings(currentForm);

            var forms = GetFormOrder();
            currentForm.Close();
            forms[formNumber + 1]?.Show();
        }

        public static void PreviousForm(int formNumber, Form currentForm)
        {
            // Сохраняем настройки текущей формы перед закрытием
            SaveFormSettings(currentForm);

            var forms = GetFormOrder();
            currentForm.Close();
            forms[formNumber - 1]?.Show();
        }

        private static List<Form> GetFormOrder()
        {
            return new List<Form> { null, new PermanentPopulationForm(), new EconomyEmploedForm(), new ForecastionForm(), null };
        }

        /// <summary>
        /// Сохраняет настройки формы перед закрытием
        /// </summary>
        /// <param name="form">Форма, настройки которой нужно сохранить</param>
        private static void SaveFormSettings(Form form)
        {
            // Используем рефлексию для вызова метода SaveSettings, если он существует
            MethodInfo saveMethod = form.GetType().GetMethod("SaveSettings", BindingFlags.Instance | BindingFlags.NonPublic);
            saveMethod?.Invoke(form, null);
        }
    }
}
