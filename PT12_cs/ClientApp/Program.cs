using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

public class Client
{
    private TcpClient client;
    private NetworkStream stream;

    public Client(string host, int port)
    {
        client = new TcpClient(host, port);
        stream = client.GetStream();
    }

    public void SendMage(Mage mage) // serializacja i wysłanie maga
    {
        string jsonString = JsonSerializer.Serialize(mage);
        byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
        stream.Write(buffer, 0, buffer.Length);
    }

    public Mage ReceiveMage() // deserializacja otrzymanego maga
    {
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        return JsonSerializer.Deserialize<Mage>(jsonString);
    }

    public void Close()
    {
        stream.Close();
        client.Close();
    }

    public void Start()
    {
        Console.WriteLine("Dostępne komendy: send/exit:");
        while (true)
        {
            string command = Console.ReadLine();
            string[] parts = command.ToLower().Split(' ');

            if (parts[0] == "exit")
            {
                Close();
                break;
            }
            else if (parts[0] == "send")
            {
                if (parts.Length < 4)
                {
                    Console.WriteLine("Nieprawidłowa komenda. Format: send <name> <level> <age>");
                    continue;
                }

                string name = parts[1];
                if (!int.TryParse(parts[2], out int level))
                {
                    Console.WriteLine("Zła wartość levela.");
                    continue;
                }
                if (!int.TryParse(parts[3], out int age))
                {
                    Console.WriteLine("Zła wartość wieku.");
                    continue;
                }

                Mage mage = new Mage(level, name, age);
                SendMage(mage);
                Mage receivedMage = ReceiveMage();
                Console.WriteLine("Klient otrzymał: " + receivedMage);
            }
            else
            {
                Console.WriteLine("Nie ma takiej komendy");
            }
        }
    }


    // Metoda Main jako punkt wejścia do aplikacji klienta
    public static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: Client <host> <port>");
            return;
        }

        string host = args[0];
        int port = int.Parse(args[1]);

        Client client = new Client(host, port);
        client.Start();
    }
}

[Serializable]
public class Mage
{
    public int Level { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public Mage(int level, string name, int age)
    {
        Level = level;
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return $"Mage: {Name}, Level: {Level}, Age: {Age}";
    }
}
