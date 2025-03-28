using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace PT10
{
    public class Engine : IComparable<Engine>
    {
        public double Displacement { get; set; }
        public double HorsePower { get; set; }
        public string Model { get; set; }

        public Engine(double displacement, double horsePower, string model)
        {
            Displacement = displacement;
            HorsePower = horsePower;
            Model = model;
        }

        public Engine() { }

        public int CompareTo(Engine other)
        {
            if (other == null) return 1;
            return this.HorsePower.CompareTo(other.HorsePower);
        }
    }


    public class Car
    {
        public string Model { get; set; }
        public Engine Motor { get; set; }
        public int Year { get; set; }

        public string EngineModel
        {
            get { return Motor?.Model; }
            set { Motor.Model = value; }
        }

        public double Displacement
        {
            get { return Motor?.Displacement ?? 0; }
            set { Motor.Displacement = value; }
        }

        public double HorsePower
        {
            get { return Motor?.HorsePower ?? 0; }
            set { Motor.HorsePower = value; }
        }

        public Car() { }

        public Car(string model, Engine motor, int year)
        {
            Model = model;
            Motor = motor;
            Year = year;
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Form form = new Form1();
            Application.Run(form);

        }
    }
}