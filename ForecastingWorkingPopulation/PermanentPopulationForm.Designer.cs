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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            chartArea5.Name = "ChartArea1";
            lifeExpectancyCoefficient.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            lifeExpectancyCoefficient.Legends.Add(legend5);
            lifeExpectancyCoefficient.Location = new Point(12, 532);
            lifeExpectancyCoefficient.Name = "lifeExpectancyCoefficient";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.Name = "Series2";
            lifeExpectancyCoefficient.Series.Add(series5);
            lifeExpectancyCoefficient.Size = new Size(857, 517);
            lifeExpectancyCoefficient.TabIndex = 0;
            lifeExpectancyCoefficient.Text = "Коэффициенты продолжительности жизни";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(875, 569);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(875, 600);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(15, 14);
            checkBox1.TabIndex = 2;
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(875, 622);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 3;
            label2.Text = "label2";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(875, 647);
            numericUpDown1.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 23);
            numericUpDown1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(875, 532);
            button1.Name = "button1";
            button1.Size = new Size(217, 23);
            button1.TabIndex = 5;
            button1.Text = "Применить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(875, 691);
            numericUpDown2.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(120, 23);
            numericUpDown2.TabIndex = 6;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(875, 735);
            numericUpDown3.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(120, 23);
            numericUpDown3.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(875, 673);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 8;
            label3.Text = "label3";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(875, 717);
            label4.Name = "label4";
            label4.Size = new Size(38, 15);
            label4.TabIndex = 9;
            label4.Text = "label4";
            // 
            // PermanentPopulation
            // 
            chartArea6.Name = "ChartArea1";
            PermanentPopulation.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            PermanentPopulation.Legends.Add(legend6);
            PermanentPopulation.Location = new Point(12, 12);
            PermanentPopulation.Name = "PermanentPopulation";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            PermanentPopulation.Series.Add(series6);
            PermanentPopulation.Size = new Size(857, 514);
            PermanentPopulation.TabIndex = 10;
            PermanentPopulation.Text = "Постоянное население";
            // 
            // genderComboBox
            // 
            genderComboBox.FormattingEnabled = true;
            genderComboBox.Location = new Point(875, 50);
            genderComboBox.Name = "genderComboBox";
            genderComboBox.Size = new Size(121, 23);
            genderComboBox.TabIndex = 11;
            // 
            // smoothingComboBox
            // 
            smoothingComboBox.FormattingEnabled = true;
            smoothingComboBox.Location = new Point(875, 100);
            smoothingComboBox.Name = "smoothingComboBox";
            smoothingComboBox.Size = new Size(121, 23);
            smoothingComboBox.TabIndex = 12;
            // 
            // windowSizeNumericUpDown
            // 
            windowSizeNumericUpDown.Location = new Point(875, 150);
            windowSizeNumericUpDown.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            windowSizeNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            windowSizeNumericUpDown.Name = "windowSizeNumericUpDown";
            windowSizeNumericUpDown.Size = new Size(121, 23);
            windowSizeNumericUpDown.TabIndex = 13;
            windowSizeNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // labelGender
            // 
            labelGender.AutoSize = true;
            labelGender.Location = new Point(875, 32);
            labelGender.Name = "labelGender";
            labelGender.Size = new Size(30, 15);
            labelGender.TabIndex = 14;
            labelGender.Text = "Пол";
            // 
            // labelSmoothing
            // 
            labelSmoothing.AutoSize = true;
            labelSmoothing.Location = new Point(875, 82);
            labelSmoothing.Name = "labelSmoothing";
            labelSmoothing.Size = new Size(81, 15);
            labelSmoothing.TabIndex = 15;
            labelSmoothing.Text = "Сглаживание";
            // 
            // labelWindowSize
            // 
            labelWindowSize.AutoSize = true;
            labelWindowSize.Location = new Point(875, 132);
            labelWindowSize.Name = "labelWindowSize";
            labelWindowSize.Size = new Size(76, 15);
            labelWindowSize.TabIndex = 16;
            labelWindowSize.Text = "Размер окна";
            // 
            // forecastionInOneAge
            // 
            chartArea7.Name = "ChartArea1";
            forecastionInOneAge.ChartAreas.Add(chartArea7);
            legend7.Name = "Legend1";
            forecastionInOneAge.Legends.Add(legend7);
            forecastionInOneAge.Location = new Point(1096, 12);
            forecastionInOneAge.Name = "forecastionInOneAge";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Legend = "Legend1";
            series7.Name = "Series2";
            forecastionInOneAge.Series.Add(series7);
            forecastionInOneAge.Size = new Size(1026, 514);
            forecastionInOneAge.TabIndex = 17;
            forecastionInOneAge.Text = "chart1";
            // 
            // forecastinForOneYear
            // 
            chartArea8.Name = "ChartArea1";
            forecastinForOneYear.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            forecastinForOneYear.Legends.Add(legend8);
            forecastinForOneYear.Location = new Point(1096, 532);
            forecastinForOneYear.Name = "forecastinForOneYear";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Legend = "Legend1";
            series8.Name = "Series2";
            forecastinForOneYear.Series.Add(series8);
            forecastinForOneYear.Size = new Size(1026, 517);
            forecastinForOneYear.TabIndex = 18;
            forecastinForOneYear.Text = "chart2";
            // 
            // PermanentPopulationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2134, 1061);
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
        private Label labelGender;
        private Label labelSmoothing;
        private Label labelWindowSize;
        private System.Windows.Forms.DataVisualization.Charting.Chart forecastionInOneAge;
        private System.Windows.Forms.DataVisualization.Charting.Chart forecastinForOneYear;

        // Элементы управления для годов, которые создаются динамически
        // Они объявлены здесь для поддержки дизайнера
    }
}
