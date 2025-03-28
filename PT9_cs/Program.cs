using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

public class Engine
{
    public double Displacement { get; set; }
    public double HorsePower { get; set; }
    [XmlAttribute("model")] // konwersja do atrybutu
    public string Model { get; set; }
    public Engine(double displacement, double horsePower, string model)
    {
        Displacement = displacement;
        HorsePower = horsePower;
        Model = model;
    }
    public Engine() { }
}

[XmlRoot("cars")] // root jako cars
[XmlType("car")] // zmiana nazwy na car
public class Car
{
    public string Model { get; set; }
    [XmlElement("engine")] // zmiana nazwy na engine
    public Engine Motor { get; set; }
    public int Year { get; set; }
    public Car() { }
    public Car(string model, Engine motor, int year)
    {
        Model = model;
        Motor = motor;
        Year = year;
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Car> myCars = new List<Car>() // lista elementów
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

        

        SerializeToXml(myCars, "myCars.xml"); // serializacja do pliku XML

        XElement rootNode = XElement.Load("myCars.xml"); // zapytanie o średnią ilość koni mechanicznych dla silników oprócz TDI
        double avgHP = (double)rootNode.XPathEvaluate("sum(/car[engine/@model != 'TDI']/engine/HorsePower) div count(/car[engine/@model != 'TDI']/engine)");
        Console.WriteLine($"Srednia ilosc koni mechanicznych (oprocz TDI): {avgHP}");

        IEnumerable<XElement> models = rootNode.XPathSelectElements("/car/Model[not(. = following::Model)]"); // wypisanie unikalnych modeli aut, porównywanie ze wszystkimi następnikami
        Console.WriteLine("\nUnikalne modele aut:");
        foreach (var model in models)
        {
            Console.WriteLine(model.Value);
        }


        List<Car> deserializedCars = DeserializeFromXml("myCars.xml"); // deserializacja

        var query1 = deserializedCars // projekcja modelu A6 na typ anonimowy o własnościach engineType i hppl
           .Where(c => c.Model == "A6")
           .Select(c => new
           {
               engineType = c.Motor.Model == "TDI" ? "diesel" : "petrol",
               hppl = c.Motor.HorsePower / c.Motor.Displacement
           });

        var groupedData = query1.GroupBy(c => c.engineType); // grupowanie po typie silnika

        foreach (var group in groupedData) // wyświetlenie danych (średnia wartość hppl dla danego typu silnika)
        {
            Console.WriteLine($"{group.Key}: {group.Average(c => c.hppl)}");
        }

        createXmlFromLinq(myCars); // XML z linq
        createXhtmlFromLinq(myCars);
        ModifyXmlDocument("myCars.xml", "ModifiedCars.xml"); // modyfikacja istniejącego XML i utworzenie nowego zmodyfikowanego
    }

    static void SerializeToXml(List<Car> cars, string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, cars);
        }
    }

    static List<Car> DeserializeFromXml(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
        using (StreamReader reader = new StreamReader(filePath))
        {
            return (List<Car>)serializer.Deserialize(reader);
        }
    }

    static void createXmlFromLinq(List<Car> myCars)
    {
        IEnumerable<XElement> nodes = myCars.Select(car => // zapytanie select linq
            new XElement("car",
                new XElement("Model", car.Model),
                new XElement("engine",
                    new XAttribute("model", car.Motor.Model),
                    new XElement("Displacement", car.Motor.Displacement),
                    new XElement("HorsePower", car.Motor.HorsePower)
                ),
                new XElement("Year", car.Year)
            )
        );

        XElement rootNode = new XElement("cars", nodes);
        rootNode.Save("CarsFromLinq.xml");
    }

    static void createXhtmlFromLinq(List<Car> myCars)
    {
        XDocument xhtmlTemplate = XDocument.Load("template.html"); // załadowanie szablonu

        var tableRows = myCars.Select(car => // wiersze tabeli na podstawie myCars
            new XElement("tr",
                new XElement("td", car.Model),
                new XElement("td", car.Motor.Model),
                new XElement("td", car.Motor.Displacement),
                new XElement("td", car.Motor.HorsePower),
                new XElement("td", car.Year)
            )
        );

        XElement tbodyElement = xhtmlTemplate.Descendants("tbody").FirstOrDefault(); // znajdujemy odpowiednie miejsce (tbody)
        tbodyElement.Add(tableRows); // dodajemy wiersze
        xhtmlTemplate.Save("CarsTable.html"); // nowy plik
    }

    static void ModifyXmlDocument(string sourceFilePath, string destinationFilePath)
    {
        XDocument xmlDoc = XDocument.Load(sourceFilePath);
        foreach (var horsePowerElement in xmlDoc.Descendants("HorsePower")) // zamiana nazwy na hp
        {
            horsePowerElement.Name = "hp";
        }
        foreach (var carElement in xmlDoc.Descendants("car")) // zamiana year na atrybut
        {
            int yearValue = int.Parse(carElement.Element("Year").Value);
            XElement modelElement = carElement.Element("Model");
            modelElement.SetAttributeValue("Year", yearValue);
            carElement.Element("Year").Remove();
        }

        xmlDoc.Save(destinationFilePath); // zapis jako nowy dokument
    }


}
