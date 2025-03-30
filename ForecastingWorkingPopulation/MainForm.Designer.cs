namespace ForecastingWorkingPopulation
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chart2).BeginInit();
            SuspendLayout();
            //
            // chart1
            //
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(12, 12);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series2";
            chart1.Series.Add(series1);
            chart1.Size = new Size(1020, 463);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            //
            // comboBox1
            //
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(911, 452);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 1;
            //
            // comboBox2
            //
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(911, 423);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(121, 23);
            comboBox2.TabIndex = 2;
            //
            // chart2
            //
            chartArea2.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            chart2.Legends.Add(legend2);
            chart2.Location = new Point(12, 481);
            chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            chart2.Series.Add(series2);
            chart2.Size = new Size(1020, 522);
            chart2.TabIndex = 3;
            chart2.Text = "chart2";
            //
            // btnLifeExpectancy
            //
            btnLifeExpectancy = new Button();
            btnLifeExpectancy.Location = new Point(911, 394);
            btnLifeExpectancy.Name = "btnLifeExpectancy";
            btnLifeExpectancy.Size = new Size(121, 23);
            btnLifeExpectancy.TabIndex = 4;
            btnLifeExpectancy.Text = "Коэффициенты";
            btnLifeExpectancy.UseVisualStyleBackColor = true;
            btnLifeExpectancy.Click += btnLifeExpectancy_Click;
            //
            // windowSizeNumericUpDown
            //
            windowSizeNumericUpDown = new NumericUpDown();
            windowSizeNumericUpDown.Location = new Point(911, 365);
            windowSizeNumericUpDown.Name = "windowSizeNumericUpDown";
            windowSizeNumericUpDown.Size = new Size(121, 23);
            windowSizeNumericUpDown.TabIndex = 5;
            windowSizeNumericUpDown.Minimum = 1;
            windowSizeNumericUpDown.Maximum = 12;
            windowSizeNumericUpDown.Value = 5;
            windowSizeNumericUpDown.ValueChanged += WindowSizeNumericUpDown_ValueChanged;
            //
            // windowSizeLabel
            //
            windowSizeLabel = new Label();
            windowSizeLabel.AutoSize = true;
            windowSizeLabel.Location = new Point(911, 347);
            windowSizeLabel.Name = "windowSizeLabel";
            windowSizeLabel.Size = new Size(121, 15);
            windowSizeLabel.TabIndex = 6;
            windowSizeLabel.Text = "Размер окна";
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2100, 1015);
            Controls.Add(windowSizeLabel);
            Controls.Add(windowSizeNumericUpDown);
            Controls.Add(btnLifeExpectancy);
            Controls.Add(chart2);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(chart1);
            Name = "MainForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ((System.ComponentModel.ISupportInitialize)chart2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private Button btnLifeExpectancy;
        private NumericUpDown windowSizeNumericUpDown;
        private Label windowSizeLabel;
    }
}
