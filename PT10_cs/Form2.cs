using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PT10
{
    public partial class Form2 : Form
    {
        private Label labelDetails;
        public Form2()
        {
            InitializeComponent2();
        }
        public void SetCarDetails(Car car) // wypisanie pojedynczego auta
        {
            labelDetails.Text = $"Model: {car.Model} Engine: {car.Motor.Model} Year: {car.Year} EngineMode: {car.EngineModel} Displacement: {car.Displacement} HorsePower: {car.HorsePower}";
        }

        public void SetCarDetails(List<Car> cars) // wypisanie listy aut
        {
            if (cars == null || cars.Count == 0)
            {
                labelDetails.Text = "No cars available.";
                return;
            }

            string details = "";

            foreach (var car in cars)
            {
                details += $"Model: {car.Model} Engine: {car.Motor.Model} Year: {car.Year} EngineMode: {car.EngineModel} Displacement: {car.Displacement} HorsePower: {car.HorsePower}\n";
            }

            labelDetails.Text = details;
        }

        private void InitializeComponent2()
        {
            this.labelDetails = new Label();
            this.SuspendLayout();

            // ustawienia labela
            this.labelDetails.AutoSize = true;
            this.labelDetails.Location = new System.Drawing.Point(13, 13);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.TabIndex = 0;
            this.labelDetails.MaximumSize = new System.Drawing.Size(800, 0);
            
            // ustawienia formularza
            this.ClientSize = new System.Drawing.Size(500, 150);
            this.Controls.Add(this.labelDetails);
            this.Name = "CarDetailsForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
