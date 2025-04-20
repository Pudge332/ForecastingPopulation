namespace ForecastingWorkingPopulation
{
    partial class ForecastionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            forecast = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)forecast).BeginInit();
            SuspendLayout();
            // 
            // forecast
            // 
            chartArea1.Name = "ChartArea1";
            forecast.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            forecast.Legends.Add(legend1);
            forecast.Location = new Point(12, 12);
            forecast.Name = "forecast";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            forecast.Series.Add(series1);
            forecast.Size = new Size(2081, 746);
            forecast.TabIndex = 0;
            forecast.Text = "chart1";
            // 
            // ForecastionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2105, 923);
            Controls.Add(forecast);
            Name = "ForecastionForm";
            Text = "ForecastionForm";
            ((System.ComponentModel.ISupportInitialize)forecast).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart forecast;
    }
}