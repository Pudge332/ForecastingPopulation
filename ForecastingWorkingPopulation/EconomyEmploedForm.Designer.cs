namespace ForecastingWorkingPopulation
{
    partial class EconomyEmploedForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            economyEmploed = new System.Windows.Forms.DataVisualization.Charting.Chart();
            inEconomyLevelSmooth = new System.Windows.Forms.DataVisualization.Charting.Chart();
            inEconomySmoothingLabel = new Label();
            inEconomyLevel = new System.Windows.Forms.DataVisualization.Charting.Chart();
            economyEmploedSmooth = new System.Windows.Forms.DataVisualization.Charting.Chart();
            genderComboBox = new ComboBox();
            smoothingComboBox = new ComboBox();
            windowSizeNumericUpDown = new NumericUpDown();
            windowSizeLabel = new Label();
            genderLabel = new Label();
            smoothingLabel = new Label();
            inEconomySmoothingComboBox = new ComboBox();
            inEconomyWindowSizeNumericUpDown = new NumericUpDown();
            inEconomyWindowSizeLabel = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)economyEmploed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)inEconomyLevelSmooth).BeginInit();
            inEconomyLevelSmooth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)inEconomyLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)economyEmploedSmooth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)windowSizeNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)inEconomyWindowSizeNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // economyEmploed
            // 
            chartArea1.Name = "ChartArea1";
            economyEmploed.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            economyEmploed.Legends.Add(legend1);
            economyEmploed.Location = new Point(3, 12);
            economyEmploed.Name = "economyEmploed";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            economyEmploed.Series.Add(series1);
            economyEmploed.Size = new Size(818, 406);
            economyEmploed.TabIndex = 0;
            economyEmploed.Text = "EconomyEmploed";
            // 
            // inEconomyLevelSmooth
            // 
            chartArea2.Name = "ChartArea1";
            inEconomyLevelSmooth.ChartAreas.Add(chartArea2);
            inEconomyLevelSmooth.Controls.Add(inEconomySmoothingLabel);
            legend2.Name = "Legend1";
            inEconomyLevelSmooth.Legends.Add(legend2);
            inEconomyLevelSmooth.Location = new Point(860, 424);
            inEconomyLevelSmooth.Name = "inEconomyLevelSmooth";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            inEconomyLevelSmooth.Series.Add(series2);
            inEconomyLevelSmooth.Size = new Size(858, 484);
            inEconomyLevelSmooth.TabIndex = 1;
            inEconomyLevelSmooth.Text = "InEconomyLevelSmooth";
            // 
            // inEconomySmoothingLabel
            // 
            inEconomySmoothingLabel.AutoSize = true;
            inEconomySmoothingLabel.Location = new Point(700, 482);
            inEconomySmoothingLabel.Name = "inEconomySmoothingLabel";
            inEconomySmoothingLabel.Size = new Size(81, 15);
            inEconomySmoothingLabel.TabIndex = 12;
            inEconomySmoothingLabel.Text = "Сглаживание";
            // 
            // inEconomyLevel
            // 
            chartArea3.Name = "ChartArea1";
            inEconomyLevel.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            inEconomyLevel.Legends.Add(legend3);
            inEconomyLevel.Location = new Point(3, 424);
            inEconomyLevel.Name = "inEconomyLevel";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            inEconomyLevel.Series.Add(series3);
            inEconomyLevel.Size = new Size(818, 484);
            inEconomyLevel.TabIndex = 2;
            inEconomyLevel.Text = "InEconomyLevel";
            // 
            // economyEmploedSmooth
            // 
            chartArea4.Name = "ChartArea1";
            economyEmploedSmooth.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            economyEmploedSmooth.Legends.Add(legend4);
            economyEmploedSmooth.Location = new Point(860, 12);
            economyEmploedSmooth.Name = "economyEmploedSmooth";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            economyEmploedSmooth.Series.Add(series4);
            economyEmploedSmooth.Size = new Size(858, 406);
            economyEmploedSmooth.TabIndex = 3;
            economyEmploedSmooth.Text = "economyEmploedSmooth";
            // 
            // genderComboBox
            // 
            genderComboBox.FormattingEnabled = true;
            genderComboBox.Location = new Point(1583, 225);
            genderComboBox.Name = "genderComboBox";
            genderComboBox.Size = new Size(121, 23);
            genderComboBox.TabIndex = 4;
            // 
            // smoothingComboBox
            // 
            smoothingComboBox.FormattingEnabled = true;
            smoothingComboBox.Location = new Point(1583, 279);
            smoothingComboBox.Name = "smoothingComboBox";
            smoothingComboBox.Size = new Size(121, 23);
            smoothingComboBox.TabIndex = 5;
            // 
            // windowSizeNumericUpDown
            // 
            windowSizeNumericUpDown.Location = new Point(1583, 334);
            windowSizeNumericUpDown.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            windowSizeNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            windowSizeNumericUpDown.Name = "windowSizeNumericUpDown";
            windowSizeNumericUpDown.Size = new Size(121, 23);
            windowSizeNumericUpDown.TabIndex = 6;
            windowSizeNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // windowSizeLabel
            // 
            windowSizeLabel.AutoSize = true;
            windowSizeLabel.Location = new Point(1583, 316);
            windowSizeLabel.Name = "windowSizeLabel";
            windowSizeLabel.Size = new Size(76, 15);
            windowSizeLabel.TabIndex = 7;
            windowSizeLabel.Text = "Размер окна";
            // 
            // genderLabel
            // 
            genderLabel.AutoSize = true;
            genderLabel.Location = new Point(1583, 207);
            genderLabel.Name = "genderLabel";
            genderLabel.Size = new Size(30, 15);
            genderLabel.TabIndex = 8;
            genderLabel.Text = "Пол";
            // 
            // smoothingLabel
            // 
            smoothingLabel.AutoSize = true;
            smoothingLabel.Location = new Point(1583, 261);
            smoothingLabel.Name = "smoothingLabel";
            smoothingLabel.Size = new Size(81, 15);
            smoothingLabel.TabIndex = 9;
            smoothingLabel.Text = "Сглаживание";
            // 
            // inEconomySmoothingComboBox
            // 
            inEconomySmoothingComboBox.FormattingEnabled = true;
            inEconomySmoothingComboBox.Location = new Point(1583, 549);
            inEconomySmoothingComboBox.Name = "inEconomySmoothingComboBox";
            inEconomySmoothingComboBox.Size = new Size(121, 23);
            inEconomySmoothingComboBox.TabIndex = 10;
            // 
            // inEconomyWindowSizeNumericUpDown
            // 
            inEconomyWindowSizeNumericUpDown.Location = new Point(1583, 617);
            inEconomyWindowSizeNumericUpDown.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            inEconomyWindowSizeNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            inEconomyWindowSizeNumericUpDown.Name = "inEconomyWindowSizeNumericUpDown";
            inEconomyWindowSizeNumericUpDown.Size = new Size(121, 23);
            inEconomyWindowSizeNumericUpDown.TabIndex = 11;
            inEconomyWindowSizeNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // inEconomyWindowSizeLabel
            // 
            inEconomyWindowSizeLabel.AutoSize = true;
            inEconomyWindowSizeLabel.Location = new Point(1588, 599);
            inEconomyWindowSizeLabel.Name = "inEconomyWindowSizeLabel";
            inEconomyWindowSizeLabel.Size = new Size(76, 15);
            inEconomyWindowSizeLabel.TabIndex = 13;
            inEconomyWindowSizeLabel.Text = "Размер окна";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1583, 531);
            label1.Name = "label1";
            label1.Size = new Size(81, 15);
            label1.TabIndex = 14;
            label1.Text = "Сглаживание";
            // 
            // EconomyEmploedForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1744, 974);
            Controls.Add(label1);
            Controls.Add(genderComboBox);
            Controls.Add(smoothingComboBox);
            Controls.Add(windowSizeNumericUpDown);
            Controls.Add(windowSizeLabel);
            Controls.Add(genderLabel);
            Controls.Add(smoothingLabel);
            Controls.Add(inEconomyWindowSizeLabel);
            Controls.Add(inEconomySmoothingComboBox);
            Controls.Add(inEconomyWindowSizeNumericUpDown);
            Controls.Add(economyEmploed);
            Controls.Add(inEconomyLevelSmooth);
            Controls.Add(inEconomyLevel);
            Controls.Add(economyEmploedSmooth);
            Name = "EconomyEmploedForm";
            Text = "EconomyEmploedForm";
            ((System.ComponentModel.ISupportInitialize)economyEmploed).EndInit();
            ((System.ComponentModel.ISupportInitialize)inEconomyLevelSmooth).EndInit();
            inEconomyLevelSmooth.ResumeLayout(false);
            inEconomyLevelSmooth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)inEconomyLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)economyEmploedSmooth).EndInit();
            ((System.ComponentModel.ISupportInitialize)windowSizeNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)inEconomyWindowSizeNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart economyEmploed;
        private System.Windows.Forms.DataVisualization.Charting.Chart inEconomyLevelSmooth;
        private System.Windows.Forms.DataVisualization.Charting.Chart inEconomyLevel;
        private System.Windows.Forms.DataVisualization.Charting.Chart economyEmploedSmooth;
        private ComboBox genderComboBox;
        private ComboBox smoothingComboBox;
        private NumericUpDown windowSizeNumericUpDown;
        private Label windowSizeLabel;
        private Label genderLabel;
        private Label smoothingLabel;
        private ComboBox inEconomySmoothingComboBox;
        private NumericUpDown inEconomyWindowSizeNumericUpDown;
        private Label inEconomySmoothingLabel;
        private Label inEconomyWindowSizeLabel;
        private Label label1;
    }
}
