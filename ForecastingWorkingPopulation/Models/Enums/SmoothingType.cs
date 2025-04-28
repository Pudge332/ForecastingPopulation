namespace ForecastingWorkingPopulation.Models.Enums
{
    public enum SmoothingType
    {
        MovingAverageWindow = 0,
        ExponentialSmoothing = 1,
        MedianFilter = 2,
        SavitzkyGolayFilter = 3
    }
}
