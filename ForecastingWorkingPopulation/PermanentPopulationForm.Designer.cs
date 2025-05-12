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
            numericUpDown1 = new NumericUpDown();
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
            NoTrim = new RadioButton();
            TrimToOne = new RadioButton();
            TrimToDelta = new RadioButton();
            label1 = new Label();
            label2 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
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
            lifeExpectancyCoefficient.Location = new Point(44, 461);
            lifeExpectancyCoefficient.Margin = new Padding(3, 2, 3, 2);
            lifeExpectancyCoefficient.Name = "lifeExpectancyCoefficient";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series2";
            lifeExpectancyCoefficient.Series.Add(series1);
            lifeExpectancyCoefficient.Size = new Size(701, 340);
            lifeExpectancyCoefficient.TabIndex = 0;
            lifeExpectancyCoefficient.Text = "Коэффициенты продолжительности жизни";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(766, 671);
            numericUpDown1.Margin = new Padding(3, 2, 3, 2);
            numericUpDown1.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(105, 23);
            numericUpDown1.TabIndex = 4;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(764, 723);
            numericUpDown2.Margin = new Padding(3, 2, 3, 2);
            numericUpDown2.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(105, 23);
            numericUpDown2.TabIndex = 6;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(764, 764);
            numericUpDown3.Margin = new Padding(3, 2, 3, 2);
            numericUpDown3.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(105, 23);
            numericUpDown3.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(766, 706);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 8;
            label3.Text = "label3";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(765, 748);
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
            PermanentPopulation.Location = new Point(44, 55);
            PermanentPopulation.Margin = new Padding(3, 2, 3, 2);
            PermanentPopulation.Name = "PermanentPopulation";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            PermanentPopulation.Series.Add(series2);
            PermanentPopulation.Size = new Size(701, 344);
            PermanentPopulation.TabIndex = 10;
            PermanentPopulation.Text = "Постоянное население";
            // 
            // genderComboBox
            // 
            genderComboBox.FormattingEnabled = true;
            genderComboBox.Location = new Point(751, 245);
            genderComboBox.Margin = new Padding(3, 2, 3, 2);
            genderComboBox.Name = "genderComboBox";
            genderComboBox.Size = new Size(106, 23);
            genderComboBox.TabIndex = 11;
            // 
            // smoothingComboBox
            // 
            smoothingComboBox.FormattingEnabled = true;
            smoothingComboBox.Location = new Point(751, 282);
            smoothingComboBox.Margin = new Padding(3, 2, 3, 2);
            smoothingComboBox.Name = "smoothingComboBox";
            smoothingComboBox.Size = new Size(106, 23);
            smoothingComboBox.TabIndex = 12;
            // 
            // windowSizeNumericUpDown
            // 
            windowSizeNumericUpDown.Location = new Point(751, 319);
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
            labelGender.Location = new Point(751, 231);
            labelGender.Name = "labelGender";
            labelGender.Size = new Size(30, 15);
            labelGender.TabIndex = 14;
            labelGender.Text = "Пол";
            // 
            // labelSmoothing
            // 
            labelSmoothing.AutoSize = true;
            labelSmoothing.Location = new Point(751, 269);
            labelSmoothing.Name = "labelSmoothing";
            labelSmoothing.Size = new Size(81, 15);
            labelSmoothing.TabIndex = 15;
            labelSmoothing.Text = "Сглаживание";
            // 
            // labelWindowSize
            // 
            labelWindowSize.AutoSize = true;
            labelWindowSize.Location = new Point(751, 306);
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
            forecastionInOneAge.Location = new Point(963, 55);
            forecastionInOneAge.Margin = new Padding(3, 2, 3, 2);
            forecastionInOneAge.Name = "forecastionInOneAge";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Series2";
            forecastionInOneAge.Series.Add(series3);
            forecastionInOneAge.Size = new Size(821, 344);
            forecastionInOneAge.TabIndex = 17;
            forecastionInOneAge.Text = "chart1";
            // 
            // forecastinForOneYear
            // 
            chartArea4.Name = "ChartArea1";
            forecastinForOneYear.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            forecastinForOneYear.Legends.Add(legend4);
            forecastinForOneYear.Location = new Point(963, 461);
            forecastinForOneYear.Margin = new Padding(3, 2, 3, 2);
            forecastinForOneYear.Name = "forecastinForOneYear";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "Series3";
            forecastinForOneYear.Series.Add(series4);
            forecastinForOneYear.Size = new Size(821, 340);
            forecastinForOneYear.TabIndex = 19;
            forecastinForOneYear.Text = "chart2";
            // 
            // btnNext
            // 
            btnNext.Location = new Point(1707, 810);
            btnNext.Margin = new Padding(3, 2, 3, 2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(131, 30);
            btnNext.TabIndex = 20;
            btnNext.Text = "Далее";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // NoTrim
            // 
            NoTrim.AutoSize = true;
            NoTrim.Location = new Point(766, 562);
            NoTrim.Name = "NoTrim";
            NoTrim.Size = new Size(176, 19);
            NoTrim.TabIndex = 21;
            NoTrim.TabStop = true;
            NoTrim.Text = "Без обрезки коэффицентов";
            NoTrim.UseVisualStyleBackColor = true;
            NoTrim.CheckedChanged += RadioButton_CheckedChanged;
            // 
            // TrimToOne
            // 
            TrimToOne.AutoSize = true;
            TrimToOne.Location = new Point(765, 602);
            TrimToOne.Name = "TrimToOne";
            TrimToOne.Size = new Size(139, 19);
            TrimToOne.TabIndex = 22;
            TrimToOne.TabStop = true;
            TrimToOne.Text = "Обрезка до единицы";
            TrimToOne.UseVisualStyleBackColor = true;
            TrimToOne.CheckedChanged += RadioButton_CheckedChanged;
            // 
            // TrimToDelta
            // 
            TrimToDelta.AutoSize = true;
            TrimToDelta.Location = new Point(765, 642);
            TrimToDelta.Name = "TrimToDelta";
            TrimToDelta.Size = new Size(104, 19);
            TrimToDelta.TabIndex = 23;
            TrimToDelta.TabStop = true;
            TrimToDelta.Text = "Обрезать до 1 ";
            TrimToDelta.UseVisualStyleBackColor = true;
            TrimToDelta.CheckedChanged += RadioButton_CheckedChanged;
            // 
            // label1
            // 
            label1.Location = new Point(765, 498);
            label1.Name = "label1";
            label1.Size = new Size(167, 55);
            label1.TabIndex = 24;
            label1.Text = "Коэффициенты для рассчета взвешенного среднего КПЖ по годам ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(219, 424);
            label2.Name = "label2";
            label2.Size = new Size(240, 15);
            label2.TabIndex = 25;
            label2.Text = "Коэффиценты продолжительности жизни";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(219, 20);
            label5.Name = "label5";
            label5.Size = new Size(291, 15);
            label5.TabIndex = 26;
            label5.Text = "Ретроспективные данные о постоянном населении";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(1216, 20);
            label6.Name = "label6";
            label6.Size = new Size(313, 15);
            label6.TabIndex = 27;
            label6.Text = "Возрастной профиль прогноза постоянного населения";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(1281, 424);
            label7.Name = "label7";
            label7.Size = new Size(189, 15);
            label7.TabIndex = 28;
            label7.Text = "Прогноз постоянного населения";
            // 
            // PermanentPopulationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1850, 871);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TrimToDelta);
            Controls.Add(TrimToOne);
            Controls.Add(NoTrim);
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
            Controls.Add(numericUpDown1);
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
        private NumericUpDown numericUpDown1;
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
        private RadioButton NoTrim;
        private RadioButton TrimToOne;
        private RadioButton TrimToDelta;
        private Label label1;
        private Label label2;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}
