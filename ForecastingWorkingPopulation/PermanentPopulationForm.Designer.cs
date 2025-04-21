namespace ForecastingWorkingPopulation
{
    partial class PermanentPopulationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Элементы управления для годов
        /// </summary>
        private Dictionary<int, YearControl> _yearControlsDesigner;

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
            lifeExpectancyCoefficient = new System.Windows.Forms.DataVisualization.Charting.Chart();
            label1 = new Label();
            checkBox1 = new CheckBox();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            button1 = new Button();
            numericUpDown2 = new NumericUpDown();
            numericUpDown3 = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            PermanentPopulation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            genderComboBox = new ComboBox();
            smoothingComboBox = new ComboBox();
            windowSizeNumericUpDown = new NumericUpDown();
            labelGender = new Label();
            labelSmoothing = new Label();
            labelWindowSize = new Label();
            forecastionInOneAge = new System.Windows.Forms.DataVisualization.Charting.Chart();
            forecastinForOneYear = new System.Windows.Forms.DataVisualization.Charting.Chart();
            btnNext = new Button();
            ((System.ComponentModel.ISupportInitialize)lifeExpectancyCoefficient).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PermanentPopulation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)windowSizeNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)forecastionInOneAge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)forecastinForOneYear).BeginInit();
            SuspendLayout();
            // 
            // lifeExpectancyCoefficient
            // 
            chartArea1.Name = "ChartArea1";
            lifeExpectancyCoefficient.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            lifeExpectancyCoefficient.Legends.Add(legend1);
            lifeExpectancyCoefficient.Location = new Point(10, 399);
            lifeExpectancyCoefficient.Margin = new Padding(3, 2, 3, 2);
            lifeExpectancyCoefficient.Name = "lifeExpectancyCoefficient";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series2";
            lifeExpectancyCoefficient.Series.Add(series1);
            lifeExpectancyCoefficient.Size = new Size(750, 388);
            lifeExpectancyCoefficient.TabIndex = 0;
            lifeExpectancyCoefficient.Text = "Коэффициенты продолжительности жизни";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(766, 427);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(766, 450);
            checkBox1.Margin = new Padding(3, 2, 3, 2);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(15, 14);
            checkBox1.TabIndex = 2;
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(766, 466);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 3;
            label2.Text = "label2";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(766, 485);
            numericUpDown1.Margin = new Padding(3, 2, 3, 2);
            numericUpDown1.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(105, 23);
            numericUpDown1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(766, 399);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(190, 26);
            button1.TabIndex = 5;
            button1.Text = "Применить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(766, 518);
            numericUpDown2.Margin = new Padding(3, 2, 3, 2);
            numericUpDown2.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(105, 23);
            numericUpDown2.TabIndex = 6;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(766, 551);
            numericUpDown3.Margin = new Padding(3, 2, 3, 2);
            numericUpDown3.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(105, 23);
            numericUpDown3.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(766, 505);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 8;
            label3.Text = "label3";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(766, 538);
            label4.Name = "label4";
            label4.Size = new Size(38, 15);
            label4.TabIndex = 9;
            label4.Text = "label4";
            // 
            // PermanentPopulation
            // 
            chartArea2.Name = "ChartArea1";
            PermanentPopulation.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            PermanentPopulation.Legends.Add(legend2);
            PermanentPopulation.Location = new Point(10, 9);
            PermanentPopulation.Margin = new Padding(3, 2, 3, 2);
            PermanentPopulation.Name = "PermanentPopulation";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            PermanentPopulation.Series.Add(series2);
            PermanentPopulation.Size = new Size(750, 386);
            PermanentPopulation.TabIndex = 10;
            PermanentPopulation.Text = "Постоянное население";
            // 
            // genderComboBox
            // 
            genderComboBox.FormattingEnabled = true;
            genderComboBox.Location = new Point(766, 38);
            genderComboBox.Margin = new Padding(3, 2, 3, 2);
            genderComboBox.Name = "genderComboBox";
            genderComboBox.Size = new Size(106, 23);
            genderComboBox.TabIndex = 11;
            // 
            // smoothingComboBox
            // 
            smoothingComboBox.FormattingEnabled = true;
            smoothingComboBox.Location = new Point(766, 75);
            smoothingComboBox.Margin = new Padding(3, 2, 3, 2);
            smoothingComboBox.Name = "smoothingComboBox";
            smoothingComboBox.Size = new Size(106, 23);
            smoothingComboBox.TabIndex = 12;
            // 
            // windowSizeNumericUpDown
            // 
            windowSizeNumericUpDown.Location = new Point(766, 112);
            windowSizeNumericUpDown.Margin = new Padding(3, 2, 3, 2);
            windowSizeNumericUpDown.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            windowSizeNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            windowSizeNumericUpDown.Name = "windowSizeNumericUpDown";
            windowSizeNumericUpDown.Size = new Size(106, 23);
            windowSizeNumericUpDown.TabIndex = 13;
            windowSizeNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // labelGender
            // 
            labelGender.AutoSize = true;
            labelGender.Location = new Point(766, 24);
            labelGender.Name = "labelGender";
            labelGender.Size = new Size(30, 15);
            labelGender.TabIndex = 14;
            labelGender.Text = "Пол";
            // 
            // labelSmoothing
            // 
            labelSmoothing.AutoSize = true;
            labelSmoothing.Location = new Point(766, 62);
            labelSmoothing.Name = "labelSmoothing";
            labelSmoothing.Size = new Size(81, 15);
            labelSmoothing.TabIndex = 15;
            labelSmoothing.Text = "Сглаживание";
            // 
            // labelWindowSize
            // 
            labelWindowSize.AutoSize = true;
            labelWindowSize.Location = new Point(766, 99);
            labelWindowSize.Name = "labelWindowSize";
            labelWindowSize.Size = new Size(76, 15);
            labelWindowSize.TabIndex = 16;
            labelWindowSize.Text = "Размер окна";
            // 
            // forecastionInOneAge
            // 
            chartArea3.Name = "ChartArea1";
            forecastionInOneAge.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            forecastionInOneAge.Legends.Add(legend3);
            forecastionInOneAge.Location = new Point(959, 9);
            forecastionInOneAge.Margin = new Padding(3, 2, 3, 2);
            forecastionInOneAge.Name = "forecastionInOneAge";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Series2";
            forecastionInOneAge.Series.Add(series3);
            forecastionInOneAge.Size = new Size(898, 386);
            forecastionInOneAge.TabIndex = 17;
            forecastionInOneAge.Text = "chart1";
            // 
            // forecastinForOneYear
            // 
            chartArea4.Name = "ChartArea1";
            forecastinForOneYear.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            forecastinForOneYear.Legends.Add(legend4);
            forecastinForOneYear.Location = new Point(963, 399);
            forecastinForOneYear.Margin = new Padding(3, 2, 3, 2);
            forecastinForOneYear.Name = "forecastinForOneYear";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "Series3";
            forecastinForOneYear.Series.Add(series4);
            forecastinForOneYear.Size = new Size(875, 368);
            forecastinForOneYear.TabIndex = 19;
            forecastinForOneYear.Text = "chart2";
            // 
            // btnNext
            // 
            btnNext.Location = new Point(1707, 771);
            btnNext.Margin = new Padding(3, 2, 3, 2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(131, 30);
            btnNext.TabIndex = 20;
            btnNext.Text = "Далее";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // PermanentPopulationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1850, 820);
            Controls.Add(btnNext);
            Controls.Add(forecastinForOneYear);
            Controls.Add(forecastionInOneAge);
            Controls.Add(labelWindowSize);
            Controls.Add(labelSmoothing);
            Controls.Add(labelGender);
            Controls.Add(windowSizeNumericUpDown);
            Controls.Add(smoothingComboBox);
            Controls.Add(genderComboBox);
            Controls.Add(PermanentPopulation);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(numericUpDown3);
            Controls.Add(numericUpDown2);
            Controls.Add(button1);
            Controls.Add(numericUpDown1);
            Controls.Add(label2);
            Controls.Add(checkBox1);
            Controls.Add(label1);
            Controls.Add(lifeExpectancyCoefficient);
            Margin = new Padding(3, 2, 3, 2);
            Name = "PermanentPopulationForm";
            Text = "Постоянное население региона";
            ((System.ComponentModel.ISupportInitialize)lifeExpectancyCoefficient).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)PermanentPopulation).EndInit();
            ((System.ComponentModel.ISupportInitialize)windowSizeNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)forecastionInOneAge).EndInit();
            ((System.ComponentModel.ISupportInitialize)forecastinForOneYear).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart lifeExpectancyCoefficient;
        private Label label1;
        private CheckBox checkBox1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private Button button1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private Label label3;
        private Label label4;
        private System.Windows.Forms.DataVisualization.Charting.Chart PermanentPopulation;
        private ComboBox genderComboBox;
        private ComboBox smoothingComboBox;
        private NumericUpDown windowSizeNumericUpDown;
        private System.Windows.Forms.DataVisualization.Charting.Chart forecastionInOneAge;
        private System.Windows.Forms.DataVisualization.Charting.Chart forecastinForOneYear;
        private Button btnNext;
        private Label labelGender;
        private Label labelSmoothing;
        private Label labelWindowSize;
    }
}
