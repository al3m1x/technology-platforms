using System.ComponentModel;
using System.Linq;

namespace PT10
{
    public partial class Form1 : Form
    {
        private SortableBindingList<Car> myCarsBindingList; // customowy BindingList
        private BindingSource carBindingSource;

        public Form1()
        {
            InitializeComponent();

            List<Car> myCars = new List<Car> // tworzymy instancje klasy Car
            {
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };

            myCarsBindingList = new SortableBindingList<Car>(myCars); // tworzymy customowy BindingList
            carBindingSource = new BindingSource { DataSource = myCarsBindingList };
            dataGridView1.DataSource = carBindingSource;


            DisplayZad1v1(); // zapytanie Linq - wyra¿enia zapytañ
            DisplayZad1v2(); // zapytanie Linq - metody

            Comparison<Car> arg1 = CompareCarsByHorsepowerDescending; // instancje delegatów do Zad 2
            Predicate<Car> arg2 = IsCarEngineTDI;
            Action<Car> arg3 = DisplayCarInMessageBox;

            myCars.Sort(new Comparison<Car>(arg1));
            foreach (Car car in myCars) // wyœwietlenie rezultatów sortowania w listboxie
            {
                listBoxResults.Items.Add($" {car.Model} ({car.Year}) {car.EngineModel} {car.Displacement} HorsePower: {car.HorsePower}");
            }
            myCars.FindAll(arg2).ForEach(arg3); // wyœwietla w osobnym Formsie auta z silnikiem TDI
        }

        private void DisplayZad1v1() // wyra¿enia zapytañ
        {
            var elements = (from car in myCarsBindingList
                            where car.Model == "A6" // car model to A6
                            group car by car.Motor.Model == "TDI" ? "diesel" : "petrol" into g // dzielimy na diesel i petrol
                            orderby g.Average(car => car.Motor.HorsePower / car.Motor.Displacement) descending // sortowanie malej¹co po HPPL
                            select new
                            {
                                engineType = g.Key,
                                avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
                            }).ToList();

            listBoxResults.Items.Clear();
            listBoxResults.Items.Add("Zapytanie oparte wyra¿eniach zapytañ: ");
            foreach (var e in elements)
            {
                listBoxResults.Items.Add($"{e.engineType}: {e.avgHPPL:F2}");
            }
        }

        private void DisplayZad1v2() // metody
        {
            var groupedCars = myCarsBindingList
                .Where(car => car.Model == "A6")
                .GroupBy(car => car.Motor.Model == "TDI" ? "diesel" : "petrol")
                .Select(g => new
                {
                    engineType = g.Key,
                    avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
                })
                .OrderByDescending(e => e.avgHPPL);

            listBoxResults.Items.Add("Zapytanie oparte na metodach: ");
            foreach (var e in groupedCars)
            {
                listBoxResults.Items.Add($"{e.engineType}: {e.avgHPPL:F2}");
            }
        }

        int CompareCarsByHorsepowerDescending(Car car1, Car car2) // porównanie
        {
            return car2.Motor.HorsePower.CompareTo(car1.Motor.HorsePower);
        }

        bool IsCarEngineTDI(Car car) // sprawdzenie czy silnik to TDI
        {
            return car.Motor.Model == "TDI";
        }

        void DisplayCarInMessageBox(Car car) // wyœwietlenie auta w osobnym Formsie
        {
            Form2 detailsForm = new Form2();
            detailsForm.SetCarDetails(car);
            detailsForm.Show();
        }
        void DisplayCarInMessageBox(List<Car> cars) // wyœwietlenie wielu aut w jednym Formsie
        {
            Form2 detailsForm = new Form2();
            detailsForm.SetCarDetails(cars);
            detailsForm.Show();
        }

        private void buttonSearch_Click(object sender, EventArgs e) // event na wyszukiwanie
        {
            string searchTerm = textBoxSearch.Text;
            List<int> foundIndexes = myCarsBindingList.FindCore(searchTerm); // szukanie indeksów aut z danym atrybutem

            if (foundIndexes.Count > 0)
            {
                List<Car> carList = new List<Car>();
                foreach (int index in foundIndexes) // dodajemy auta do jednej listy aut
                {
                    Car foundCar = myCarsBindingList[index];
                    carList.Add(foundCar);
                    dataGridView1.Rows[index].Selected = true; // zaznaczanie w tabeli
                }
                DisplayCarInMessageBox(carList); // wyœwietlanie wszystkich aut w jednym Formsie
                dataGridView1.FirstDisplayedScrollingRowIndex = foundIndexes.First();
            }
            else
            {
                MessageBox.Show("Nothing found");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e) // dodanie nowego auta, potem mo¿na zmieniæ wartoœci
        {
            Car newCar = new Car("Model", new Engine(2.0, 200, "EngineModel"), 2020);
            myCarsBindingList.Add(newCar);
        }

        private void buttonDelete_Click(object sender, EventArgs e) // obs³uga usuniêcia
        {
            if (dataGridView1.CurrentRow != null) // czy wiersz istnieje
            {
                myCarsBindingList.Remove((Car)dataGridView1.CurrentRow.DataBoundItem); // usuwanie auta z bindinglisty z gridview
                carBindingSource.ResetBindings(false); // aktualizacja danych
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) // event na zmianê wartoœci komórki
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit); // zatwierdzanie edycji zmian w komórce
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) // sortowanie po wartoœci atrybutu, zajmuje siê tym funkcja Sort z customowej bindinglisty
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "Model")
            {
                myCarsBindingList.Sort<string>("Model");
            }
            else if (columnName == "Year")
            {
                myCarsBindingList.Sort<int>("Year");
            }
            else if (columnName == "HorsePower")
            {
                myCarsBindingList.Sort<double>("HorsePower");
            }
            else if (columnName == "Displacement")
            {
                myCarsBindingList.Sort<double>("Displacement");
            }
            else if (columnName == "EngineModel")
            {
                myCarsBindingList.Sort<string>("EngineModel");
            }
            carBindingSource.ResetBindings(false); // reset widoku
        }

    }
}
