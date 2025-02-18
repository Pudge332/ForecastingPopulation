using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace ForecastingWorkingPopulation.Infrastructure.GraphPainting
{
    public class LinearGraphPainter
    {
        public Series PainLinearGraph(string name, List<double> xValues, List<double> yValues)
        {
            if (xValues.Count != yValues.Count)
                return null;

            var series = new Series
            {
                Name = name,
                ChartType = SeriesChartType.Line
            };

            for(int i = 0; i < xValues.Count; i++)
                series.Points.AddXY(xValues[i], yValues[i]);
            
            return series;
        }

        public Series PainLinearGraph(string name, List<int> xValues, List<int> yValues)
        {
            if (xValues.Count != yValues.Count)
                return null;

            var doubleXValue = xValues.Select(Convert.ToDouble).ToList();
            var doubleVYalue = yValues.Select(Convert.ToDouble).ToList();

            var series = new Series
            {
                Name = name,
                ChartType = SeriesChartType.Line,
                Color = GetRandomColor()
            };

            for (int i = 0; i < xValues.Count; i++)
                series.Points.AddXY(doubleXValue[i], doubleVYalue[i]);

            return series;
        }

        private Color GetRandomColor()
        {
            var randomizer = new Random();
            var r = randomizer.Next(0, 255);
            var g = randomizer.Next(0, 255);
            var b = randomizer.Next(0, 255);

            return Color.FromArgb(255, r, b, b);
        }
    }
}
