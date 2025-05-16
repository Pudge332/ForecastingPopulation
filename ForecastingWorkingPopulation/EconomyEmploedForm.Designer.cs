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
            button1 = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            BackButton = new Button();
            numericUpDownMaxAge = new NumericUpDown();
            numericUpDownMinAge = new NumericUpDown();
            label6 = new Label();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)economyEmploed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)inEconomyLevelSmooth).BeginInit();
            inEconomyLevelSmooth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)inEconomyLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)economyEmploedSmooth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)windowSizeNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)inEconomyWindowSizeNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMaxAge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMinAge).BeginInit();
            SuspendLayout();
            // 
            // economyEmploed
            // 
            chartArea5.Name = "ChartArea1";
            economyEmploed.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            economyEmploed.Legends.Add(legend5);
            economyEmploed.Location = new Point(58, 49);
            economyEmploed.Name = "economyEmploed";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            economyEmploed.Series.Add(series5);
            economyEmploed.Size = new Size(763, 379);
            economyEmploed.TabIndex = 0;
            economyEmploed.Text = "EconomyEmploed";
            // 
            // inEconomyLevelSmooth
            // 
            chartArea6.Name = "ChartArea1";
            inEconomyLevelSmooth.ChartAreas.Add(chartArea6);
            inEconomyLevelSmooth.Controls.Add(inEconomySmoothingLabel);
            legend6.Name = "Legend1";
            inEconomyLevelSmooth.Legends.Add(legend6);
            inEconomyLevelSmooth.Location = new Point(860, 478);
            inEconomyLevelSmooth.Name = "inEconomyLevelSmooth";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            inEconomyLevelSmooth.Series.Add(series6);
            inEconomyLevelSmooth.Size = new Size(809, 443);
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
            chartArea7.Name = "ChartArea1";
            inEconomyLevel.ChartAreas.Add(chartArea7);
            legend7.Name = "Legend1";
            inEconomyLevel.Legends.Add(legend7);
            inEconomyLevel.Location = new Point(58, 476);
            inEconomyLevel.Name = "inEconomyLevel";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Legend = "Legend1";
            series7.Name = "Series1";
            inEconomyLevel.Series.Add(series7);
            inEconomyLevel.Size = new Size(763, 445);
            inEconomyLevel.TabIndex = 2;
            inEconomyLevel.Text = "InEconomyLevel";
            // 
            // economyEmploedSmooth
            // 
            chartArea8.Name = "ChartArea1";
            economyEmploedSmooth.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            economyEmploedSmooth.Legends.Add(legend8);
            economyEmploedSmooth.Location = new Point(865, 49);
            economyEmploedSmooth.Name = "economyEmploedSmooth";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Legend = "Legend1";
            series8.Name = "Series1";
            economyEmploedSmooth.Series.Add(series8);
            economyEmploedSmooth.Size = new Size(799, 379);
            economyEmploedSmooth.TabIndex = 3;
            economyEmploedSmooth.Text = "economyEmploedSmooth";
            // 
            // genderComboBox
            // 
            genderComboBox.FormattingEnabled = true;
            genderComboBox.Location = new Point(1535, 254);
            genderComboBox.Name = "genderComboBox";
            genderComboBox.Size = new Size(121, 23);
            genderComboBox.TabIndex = 4;
            // 
            // smoothingComboBox
            // 
            smoothingComboBox.FormattingEnabled = true;
            smoothingComboBox.Location = new Point(1535, 308);
            smoothingComboBox.Name = "smoothingComboBox";
            smoothingComboBox.Size = new Size(121, 23);
            smoothingComboBox.TabIndex = 5;
            // 
            // windowSizeNumericUpDown
            // 
            windowSizeNumericUpDown.Location = new Point(1535, 363);
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
            windowSizeLabel.Location = new Point(1535, 345);
            windowSizeLabel.Name = "windowSizeLabel";
            windowSizeLabel.Size = new Size(76, 15);
            windowSizeLabel.TabIndex = 7;
            windowSizeLabel.Text = "Размер окна";
            // 
            // genderLabel
            // 
            genderLabel.AutoSize = true;
            genderLabel.Location = new Point(1535, 236);
            genderLabel.Name = "genderLabel";
            genderLabel.Size = new Size(30, 15);
            genderLabel.TabIndex = 8;
            genderLabel.Text = "Пол";
            // 
            // smoothingLabel
            // 
            smoothingLabel.AutoSize = true;
            smoothingLabel.Location = new Point(1535, 290);
            smoothingLabel.Name = "smoothingLabel";
            smoothingLabel.Size = new Size(81, 15);
            smoothingLabel.TabIndex = 9;
            smoothingLabel.Text = "Сглаживание";
            // 
            // inEconomySmoothingComboBox
            // 
            inEconomySmoothingComboBox.FormattingEnabled = true;
            inEconomySmoothingComboBox.Location = new Point(1535, 648);
            inEconomySmoothingComboBox.Name = "inEconomySmoothingComboBox";
            inEconomySmoothingComboBox.Size = new Size(121, 23);
            inEconomySmoothingComboBox.TabIndex = 10;
            // 
            // inEconomyWindowSizeNumericUpDown
            // 
            inEconomyWindowSizeNumericUpDown.Location = new Point(1535, 716);
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
            inEconomyWindowSizeLabel.Location = new Point(1540, 698);
            inEconomyWindowSizeLabel.Name = "inEconomyWindowSizeLabel";
            inEconomyWindowSizeLabel.Size = new Size(76, 15);
            inEconomyWindowSizeLabel.TabIndex = 13;
            inEconomyWindowSizeLabel.Text = "Размер окна";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1535, 630);
            label1.Name = "label1";
            label1.Size = new Size(81, 15);
            label1.TabIndex = 14;
            label1.Text = "Сглаживание";
            // 
            // button1
            // 
            button1.Location = new Point(1588, 887);
            button1.Name = "button1";
            button1.Size = new Size(135, 34);
            button1.TabIndex = 15;
            button1.Text = "Далее";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(334, 19);
            label2.Name = "label2";
            label2.Size = new Size(244, 15);
            label2.TabIndex = 16;
            label2.Text = "Результаты опросов численности занятых ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(334, 446);
            label3.Name = "label3";
            label3.Size = new Size(223, 15);
            label3.TabIndex = 17;
            label3.Text = "Уровни занятости для пронозиварония";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1203, 19);
            label4.Name = "label4";
            label4.Size = new Size(192, 15);
            label4.TabIndex = 18;
            label4.Text = "Сглаженные результаты опросов";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(1163, 446);
            label5.Name = "label5";
            label5.Size = new Size(232, 15);
            label5.TabIndex = 19;
            label5.Text = "Уровень занятости для расчета прогноза";
            // 
            // BackButton
            // 
            BackButton.Location = new Point(1447, 888);
            BackButton.Name = "BackButton";
            BackButton.Size = new Size(135, 34);
            BackButton.TabIndex = 20;
            BackButton.Text = "Назад";
            BackButton.UseVisualStyleBackColor = true;
            BackButton.Click += BackButton_Click;
            // 
            // numericUpDownMaxAge
            // 
            numericUpDownMaxAge.Location = new Point(1535, 799);
            numericUpDownMaxAge.Minimum = new decimal(new int[] { 14, 0, 0, 0 });
            numericUpDownMaxAge.Name = "numericUpDownMaxAge";
            numericUpDownMaxAge.Size = new Size(120, 23);
            numericUpDownMaxAge.TabIndex = 21;
            numericUpDownMaxAge.Value = new decimal(new int[] { 14, 0, 0, 0 });
            numericUpDownMaxAge.ValueChanged += NumericUpDownAge_ValueChanged;
            // 
            // numericUpDownMinAge
            // 
            numericUpDownMinAge.Location = new Point(1535, 758);
            numericUpDownMinAge.Minimum = new decimal(new int[] { 13, 0, 0, 0 });
            numericUpDownMinAge.Name = "numericUpDownMinAge";
            numericUpDownMinAge.Size = new Size(120, 23);
            numericUpDownMinAge.TabIndex = 22;
            numericUpDownMinAge.Value = new decimal(new int[] { 13, 0, 0, 0 });
            numericUpDownMinAge.ValueChanged += NumericUpDownAge_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(1535, 742);
            label6.Name = "label6";
            label6.Size = new Size(135, 15);
            label6.TabIndex = 23;
            label6.Text = "Минимальный возраст";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(1535, 781);
            label7.Name = "label7";
            label7.Size = new Size(139, 15);
            label7.TabIndex = 24;
            label7.Text = "Максимальный возраст";
            // 
            // EconomyEmploedForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1744, 934);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(numericUpDownMinAge);
            Controls.Add(numericUpDownMaxAge);
            Controls.Add(BackButton);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button1);
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
            ((System.ComponentModel.ISupportInitialize)numericUpDownMaxAge).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMinAge).EndInit();
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
        private Button btnNext;
        private Button btnPrev;
        private Button button1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button BackButton;
        private NumericUpDown numericUpDownMaxAge;
        private NumericUpDown numericUpDownMinAge;
        private Label label6;
        private Label label7;
    }
}
