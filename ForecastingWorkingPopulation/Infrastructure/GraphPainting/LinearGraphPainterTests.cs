using Xunit;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Collections.Generic;

namespace ForecastingWorkingPopulation.Infrastructure.GraphPainting
{
    public class LinearGraphPainterTests
    {
        private readonly LinearGraphPainter _painter;

        public LinearGraphPainterTests()
        {
            _painter = new LinearGraphPainter();
        }

        [Fact]
        public void PainLinearGraph_ValidData_CreatesSeries()
        {
            // Arrange
            var xValues = new List<double> { 1, 2, 3 };
            var yValues = new List<double> { 10, 20, 30 };

            // Act
            var result = _painter.PaintLinearGraph("Test", xValues, yValues);

            // Assert
            Assert.Equal(3, result.Points.Count);
            Assert.Equal(SeriesChartType.Line, result.ChartType);
        }

        [Fact]
        public void FixLength_UnevenLists_TrimsLongerList()
        {
            // Arrange
            var xValues = new List<int> { 1, 2, 3, 4 };
            var yValues = new List<int> { 10, 20 };

            // Act
            var (trimmedX, trimmedY) = _painter.FixLenght(xValues, yValues);

            // Assert
            Assert.Equal(2, trimmedX.Count);
            Assert.Equal(2, trimmedY.Count);
        }

        [Fact]
        public void GetRandomColor_ReturnsValidColor()
        {
            // Act
            var color = _painter.GetRandomColor();

            // Assert
            Assert.InRange(color.R, 0, 255);
            Assert.InRange(color.G, 0, 255);
            Assert.InRange(color.B, 0, 255);
        }

        [Fact]
        public void PainLinearGraph_EmptyLists_ReturnsEmptySeries()
        {
            var result = _painter.PaintLinearGraph("EmptyTest", new List<double>(), new List<double>());
            Assert.Empty(result.Points);
        }

        [Fact]
        public void PainLinearGraph_NegativeValues_HandlesCorrectly()
        {
            var x = new List<double> { -1, -2 };
            var y = new List<double> { -10, -20 };
            var result = _painter.PaintLinearGraph("NegativeTest", x, y);

            Assert.Equal(2, result.Points.Count);
            Assert.Equal(-1, result.Points[0].XValue);
            Assert.Equal(-10, result.Points[0].YValues[0]);
        }

        [Fact]
        public void PainLinearGraph_IntValues_ConvertsToDouble()
        {
            var x = new List<int> { 1, 2 };
            var y = new List<int> { 10, 20 };
            var result = _painter.PaintLinearGraph("IntTest", x, y);

            Assert.Equal(1.0, result.Points[0].XValue);
            Assert.Equal(10.0, result.Points[0].YValues[0]);
        }

        [Fact]
        public void FixLength_DoubleLists_TrimsCorrectly()
        {
            var x = new List<double> { 1.1, 2.2, 3.3 };
            var y = new List<double> { 10.1, 20.2 };
            var (trimmedX, trimmedY) = _painter.FixLenght(x, y);

            Assert.Equal(2, trimmedX.Count);
            Assert.Equal(2, trimmedY.Count);
        }

        [Fact]
        public void GetRandomColor_AlphaIsOpaque()
        {
            var color = _painter.GetRandomColor();
            Assert.Equal(255, color.A);
        }
    }
}
