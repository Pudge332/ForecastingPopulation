namespace ForecastingWorkingPopulation.Infrastructure
{
    public static class FormRouting
    {
        public static void NextForm(int formNumber, Form currentForm)
        {
            var forms = GetFormOrder();
            currentForm.Close();
            forms[formNumber + 1]?.Show();
        }

        public static void PreviousForm(int formNumber, Form currentForm)
        {
            var forms = GetFormOrder();
            currentForm.Close();
            forms[formNumber - 1]?.Show();
        }

        private static List<Form> GetFormOrder()
        {
            return new List<Form> { null, new PermanentPopulationForm(), new EconomyEmploedForm(), new ForecastionForm(), null };
        }
    }
}
