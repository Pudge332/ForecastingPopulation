using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using static OfficeOpenXml.ExcelErrorValue;

[assembly: InternalsVisibleTo("Infrastructure.Tests")]
[assembly: InternalsVisibleTo("Infrastructure.Tests.GraphPainting")]

namespace ForecastingWorkingPopulation.Infrastructure.GraphPainting
{
    public class LinearGraphPainter
    {
        public Series PainLinearGraph(string name, List<double> xValues, List<double> yValues)
        {
            if (xValues.Count != yValues.Count)
                (xValues, yValues) = FixLenght(xValues, yValues);

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
                (xValues, yValues) = FixLenght(xValues, yValues);

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

        internal Color GetRandomColor()
        {
            var randomizer = new Random();
            var r = randomizer.Next(0, 255);
            var g = randomizer.Next(0, 255);
            var b = randomizer.Next(0, 255);

            return Color.FromArgb(255, r, b, b);
        }

        internal (List<T>, List<T>) FixLenght<T>(List<T> xValues, List<T> yValues)
        {
            var resultX = new List<T>();
            var resultY = new List<T>();

            if (yValues.Count > xValues.Count)
                (resultX, resultY) = Switch(xValues, yValues);
            else
                (resultY, resultX) = Switch(yValues, xValues);

            return (resultX, resultY);
        }

        private (List<T>, List<T>) Switch<T>(List<T> firstValues, List<T> secondValues)
        {
            var firstLength = firstValues.Count;
            var secondLength = secondValues.Count;
            var newSecondValues = new List<T>();

            for (int i = 0; i < secondValues.Count; i++)
            {
                if (secondLength > firstLength)
                    secondLength--;
                else
                    newSecondValues.Add(secondValues[i]);
            }

            return (firstValues, newSecondValues);
        }
    }
}
