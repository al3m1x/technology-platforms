using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

public class Server
{
    private TcpListener listener;
    private bool running = true;
    private List<ClientHandler> clients = new List<ClientHandler>();
    private readonly object clientsLock = new object(); // Blokada do synchronizacji dostępu do listy clients

    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
        listener.Start();
        Console.WriteLine("Serwer wystartował");

        Thread acceptThread = new Thread(AcceptClients);
        acceptThread.Start();

        while (running)
        {
            string command = Console.ReadLine();
            if (command.ToLower() == "stop")
            {
                Stop();
            }
        }
    }

    private void AcceptClients()
    {
        while (running)
        {
            try
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                Console.WriteLine("Połączono z klientem");

                ClientHandler clientHandler = new ClientHandler(tcpClient, this);
                lock (clientsLock)
                {
                    clients.Add(clientHandler);
                }

                Thread clientThread = new Thread(clientHandler.HandleClient);
                clientThread.Start();
            }
            catch (SocketException)
            {
                break;
            }
        }
    }

    public void Stop()
    {
        running = false;
        listener.Stop();

        lock (clientsLock)
        {
            foreach (var client in clients)
            {
                client.Stop();
            }
        }

        Console.WriteLine("Zatrzymano serwer");
    }

    public void RemoveClient(ClientHandler client)
    {
        lock (clientsLock)
        {
            clients.Remove(client);
        }
    }
}

public class ClientHandler
{
    private TcpClient tcpClient;
    private NetworkStream stream;
    private Server server;

    public ClientHandler(TcpClient client, Server server)
    {
        tcpClient = client;
        stream = tcpClient.GetStream();
        this.server = server;
    }

    public void HandleClient()
    {
        try
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Mage mage = JsonSerializer.Deserialize<Mage>(jsonString);

                Console.WriteLine("Otrzymano: " + mage);

                mage.Level++; // zwiększamy poziom postaci

                Console.WriteLine("Zmodyfikowano do: " + mage);

                jsonString = JsonSerializer.Serialize(mage);
                buffer = Encoding.UTF8.GetBytes(jsonString);
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd: " + ex.Message);
        }
        finally
        {
            Close();
        }
    }

    public void Stop()
    {
        tcpClient.Close();
        stream.Close();
    }

    public void Close()
    {
        stream.Close();
        tcpClient.Close();
        Console.WriteLine("Rozłączono z klientem");

        server.RemoveClient(this);
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

class Program
{
    static void Main(string[] args)
    {
        Server server = new Server(5000);
        server.Start();
    }
}
