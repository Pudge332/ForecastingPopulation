using ForecastingWorkingPopulation.Infrastructure;
using System;
using System.Windows.Forms;

namespace ForecastingWorkingPopulation
{
    public partial class RegionActionForm : Form
    {
        private readonly int _regionId;
        private readonly string _regionName;

        public RegionActionForm(int regionId, string regionName)
        {
            InitializeComponent();
            _regionId = regionId;
            _regionName = regionName;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = $"Действия с регионом: {_regionName}";
            labelRegionInfo.Text = $"Выбран регион: {_regionName} (ID: {_regionId})";
        }

        private void buttonOpenMainForm_Click(object sender, EventArgs e)
        {
            // Сохраняем выбранный регион в хранилище
            CalculationStorage.Instance.CurrentRegion = _regionId;

            // Открываем MainForm
            var mainForm = new EconomyEmploedForm();
            mainForm.Show();
            this.Close();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            // Сохраняем выбранный регион в хранилище
            CalculationStorage.Instance.CurrentRegion = _regionId;

            // Закрываем форму и возвращаемся к выбору файла
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.labelRegionInfo = new System.Windows.Forms.Label();
            this.buttonOpenMainForm = new System.Windows.Forms.Button();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // labelRegionInfo
            //
            this.labelRegionInfo.AutoSize = true;
            this.labelRegionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRegionInfo.Location = new System.Drawing.Point(12, 9);
            this.labelRegionInfo.Name = "labelRegionInfo";
            this.labelRegionInfo.Size = new System.Drawing.Size(127, 17);
            this.labelRegionInfo.TabIndex = 0;
            this.labelRegionInfo.Text = "Выбран регион:";
            //
            // buttonOpenMainForm
            //
            this.buttonOpenMainForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOpenMainForm.Location = new System.Drawing.Point(12, 45);
            this.buttonOpenMainForm.Name = "buttonOpenMainForm";
            this.buttonOpenMainForm.Size = new System.Drawing.Size(300, 40);
            this.buttonOpenMainForm.TabIndex = 1;
            this.buttonOpenMainForm.Text = "Перейти к анализу региона";
            this.buttonOpenMainForm.UseVisualStyleBackColor = true;
            this.buttonOpenMainForm.Click += new System.EventHandler(this.buttonOpenMainForm_Click);
            //
            // buttonSelectFile
            //
            this.buttonSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSelectFile.Location = new System.Drawing.Point(12, 100);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(300, 40);
            this.buttonSelectFile.TabIndex = 2;
            this.buttonSelectFile.Text = "Выбрать файл для региона";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            //
            // RegionActionForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 161);
            this.Controls.Add(this.buttonSelectFile);
            this.Controls.Add(this.buttonOpenMainForm);
            this.Controls.Add(this.labelRegionInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegionActionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Действия с регионом";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelRegionInfo;
        private System.Windows.Forms.Button buttonOpenMainForm;
        private System.Windows.Forms.Button buttonSelectFile;
    }
}
