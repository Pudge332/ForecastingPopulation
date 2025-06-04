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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            forecast = new System.Windows.Forms.DataVisualization.Charting.Chart();
            btnPrev = new Button();
            comboBox1 = new ComboBox();
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            ExportButton = new Button();
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            forecastInOneAge = new System.Windows.Forms.DataVisualization.Charting.Chart();
            label3 = new Label();
            label4 = new Label();
            exitButton = new Button();
            ((System.ComponentModel.ISupportInitialize)forecast).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)forecastInOneAge).BeginInit();
            SuspendLayout();
            // 
            // forecast
            // 
            chartArea1.Name = "ChartArea1";
            forecast.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            forecast.Legends.Add(legend1);
            forecast.Location = new Point(10, 44);
            forecast.Margin = new Padding(3, 2, 3, 2);
            forecast.Name = "forecast";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            forecast.Series.Add(series1);
            forecast.Size = new Size(1020, 544);
            forecast.TabIndex = 0;
            forecast.Text = "chart1";
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(1562, 594);
            btnPrev.Margin = new Padding(3, 2, 3, 2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(131, 30);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "Назад";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += btnPrev_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(907, 471);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 2;
            comboBox1.Text = "Все";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(908, 559);
            numericUpDown1.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 23);
            numericUpDown1.TabIndex = 3;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(908, 515);
            numericUpDown2.Maximum = new decimal(new int[] { 2045, 0, 0, 0 });
            numericUpDown2.Minimum = new decimal(new int[] { 2027, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(120, 23);
            numericUpDown2.TabIndex = 4;
            numericUpDown2.Value = new decimal(new int[] { 2027, 0, 0, 0 });
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(906, 497);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 5;
            label1.Text = "До года:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(908, 541);
            label2.Name = "label2";
            label2.Size = new Size(83, 15);
            label2.TabIndex = 6;
            label2.Text = "Шаг прогноза";
            // 
            // ExportButton
            // 
            ExportButton.Location = new Point(1712, 449);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(118, 63);
            ExportButton.TabIndex = 7;
            ExportButton.Text = "Экспорт расчетов в Excel";
            ExportButton.UseVisualStyleBackColor = true;
            ExportButton.Click += button1_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // forecastInOneAge
            // 
            chartArea2.Name = "ChartArea1";
            forecastInOneAge.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            forecastInOneAge.Legends.Add(legend2);
            forecastInOneAge.Location = new Point(1048, 44);
            forecastInOneAge.Name = "forecastInOneAge";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            forecastInOneAge.Series.Add(series2);
            forecastInOneAge.Size = new Size(782, 544);
            forecastInOneAge.TabIndex = 8;
            forecastInOneAge.Text = "chart1";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1321, 24);
            label3.Name = "label3";
            label3.Size = new Size(241, 15);
            label3.TabIndex = 9;
            label3.Text = "Прогноз численности занятого населения";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(297, 24);
            label4.Name = "label4";
            label4.Size = new Size(365, 15);
            label4.TabIndex = 10;
            label4.Text = "Возрастной профиль прогноза численности занятого населения";
            // 
            // exitButton
            // 
            exitButton.Location = new Point(1699, 594);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(131, 30);
            exitButton.TabIndex = 11;
            exitButton.Text = "Выход";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += exitButton_Click;
            // 
            // ForecastionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1842, 638);
            Controls.Add(exitButton);
            Controls.Add(label4);
            Controls.Add(btnPrev);
            Controls.Add(ExportButton);
            Controls.Add(label3);
            Controls.Add(forecastInOneAge);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(comboBox1);
            Controls.Add(forecast);
            Margin = new Padding(3, 2, 3, 2);
            Name = "ForecastionForm";
            Text = "ForecastionForm";
            ((System.ComponentModel.ISupportInitialize)forecast).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)forecastInOneAge).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ExportButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart forecast;
        private Button btnPrev;
        private ComboBox comboBox1;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private Label label1;
        private Label label2;
        private OpenFileDialog openFileDialog1;
        private FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataVisualization.Charting.Chart forecastInOneAge;
        private Label label3;
        private Label label4;
        private Button exitButton;
    }
}
