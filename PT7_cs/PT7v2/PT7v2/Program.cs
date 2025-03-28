using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace ConsoleApp
{
    [Serializable]
    class MyComparator : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x.Length != y.Length)
            {
                return x.Length.CompareTo(y.Length);
            }
            else
            {
                return string.Compare(x, y);
            }
        }
    }

    public static class FileSystemInfoExtensions
    {
        public static (DateTime oldestDate, string oldestFileName) FindOldestFile(this DirectoryInfo directory)
        {
            DateTime oldestDate = DateTime.MaxValue;
            string oldestFileName = "";

            foreach (var file in directory.GetFiles())
            {
                if (file.LastWriteTime < oldestDate)
                {
                    oldestDate = file.LastWriteTime;
                    oldestFileName = file.Name;
                }
            }

            foreach (var subdirectory in directory.GetDirectories())
            {
                var (subdirectoryOldestDate, subdirectoryOldestFileName) = FindOldestFile(subdirectory);
                if (subdirectoryOldestDate < oldestDate)
                {
                    oldestDate = subdirectoryOldestDate;
                    oldestFileName = subdirectoryOldestFileName;
                }
            }

            return (oldestDate, oldestFileName);
        }

        public static string GetDosAttributes(this FileSystemInfo fileSystemInfo)
        {
            string attributes = "";

            if ((fileSystemInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes += "r";
            }
            else
            {
                attributes += "-";
            }

            if ((fileSystemInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                attributes += "h";
            }
            else
            {
                attributes += "-";
            }

            if ((fileSystemInfo.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                attributes += "s";
            }
            else
            {
                attributes += "-";
            }

            if ((fileSystemInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                attributes += "a";
            }
            else
            {
                attributes += "-";
            }
            return attributes;
        }
    }

    class Program
    {
        static void ShowCatalog(string name, int depth)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Nie podano katalogu.");
                return;
            }

            if (!Directory.Exists(name))
            {
                Console.WriteLine($"Podana ścieżka '{name}' nie istnieje lub nie jest katalogiem.");
                return;
            }

            string[] files = Directory.GetFiles(name);
            string[] directories = Directory.GetDirectories(name);
            var orderedElements = new SortedDictionary<string, long>(new MyComparator());
            
            foreach (string file in files)
            {
                for (int i = 0; i < depth; i++)
                {
                    Console.Write("  ");
                }
                FileInfo fileInfo = new FileInfo(file);
                if (depth == 0)
                {
                    orderedElements.Add(Path.GetFileName(file), fileInfo.Length);
                }
                string dosAttributes = fileInfo.GetDosAttributes();
                Console.WriteLine($"{Path.GetFileName(file)} {fileInfo.Length} bytes {dosAttributes}");
            }
            foreach (string dir in directories)
            {
                for (int i = 0; i < depth; i++)
                {
                    Console.Write("  ");
                }
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                if (depth == 0)
                {
                    orderedElements.Add(Path.GetFileName(dir), Directory.GetFileSystemEntries(dir).Length);
                }
                string dosAttributes = directoryInfo.GetDosAttributes();
                int itemCount = Directory.GetFileSystemEntries(dir).Length;
                Console.WriteLine($"{Path.GetFileName(dir)} ({itemCount}) {dosAttributes}");
                ShowCatalog(dir, depth + 1);
            }

            if (depth == 0)
            {
                SerializeCollection(orderedElements, "serialized_data.bin");

                var deserializedElements = DeserializeCollection<SortedDictionary<string, long>>("serialized_data.bin");
                Console.WriteLine("\nDeserializowana kolekcja:");
                foreach (var element in deserializedElements)
                {
                    Console.WriteLine($"{element.Key} -> {element.Value}");
                }
            }
            Console.WriteLine("");
        }

        static void SerializeCollection<T>(T collection, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create)) // using - zabezpieczenie przed nieoczekiwanym zamknięciem, błędem
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, collection);
            }
        }

        static T DeserializeCollection<T>(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

        static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);
            ShowCatalog(args[0], 0);
            DirectoryInfo directory = new DirectoryInfo(args[0]);
            var (oldestDate, oldestFileName) = directory.FindOldestFile();
            Console.WriteLine($"Najstarszy plik: {oldestFileName}, Data modyfikacji: {oldestDate}");
        }
    }
}
