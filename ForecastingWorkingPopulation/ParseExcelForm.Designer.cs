﻿namespace ForecastingWorkingPopulation
{
    partial class ParseExcelForm
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
            dataGridView1 = new DataGridView();
            openFileDialog1 = new OpenFileDialog();
            button1 = new Button();
            progressBar1 = new ProgressBar();
            button2 = new Button();
            massImportFiles = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(31, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(657, 426);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            button1.Location = new Point(694, 361);
            button1.Name = "button1";
            button1.Size = new Size(94, 77);
            button1.TabIndex = 2;
            button1.Text = "Бюллетень";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(694, 279);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(94, 23);
            progressBar1.TabIndex = 3;
            // 
            // button2
            // 
            button2.Location = new Point(694, 308);
            button2.Name = "button2";
            button2.Size = new Size(94, 47);
            button2.TabIndex = 4;
            button2.Text = "Бюллетень рождаемости";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // massImportFiles
            // 
            massImportFiles.Location = new Point(694, 12);
            massImportFiles.Name = "massImportFiles";
            massImportFiles.Size = new Size(94, 76);
            massImportFiles.TabIndex = 5;
            massImportFiles.Text = "Массово загрузить результаты опросов";
            massImportFiles.UseVisualStyleBackColor = true;
            massImportFiles.Click += massImportFiles_Click;
            // 
            // ParseExcelForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(massImportFiles);
            Controls.Add(button2);
            Controls.Add(progressBar1);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "ParseExcelForm";
            Text = "Выбор региона для загрузки данных";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private OpenFileDialog openFileDialog1;
        private Button button1;
        private ProgressBar progressBar1;
        private Button button2;
        private Button massImportFiles;
    }
}
