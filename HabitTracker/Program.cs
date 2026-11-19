using System;
using System.IO;
using System.Text;

namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string path = "example.txt";

            // 1. Запись через StreamWriter
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Первая строка");
                writer.WriteLine("Вторая строка");
            }

            // 2. Дозапись в конец файла
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine("Дописанная строка");
            }

            // 3. Построчное чтение через StreamReader
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                int num = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"{num}: {line}");
                    num++;
                }
            }

            // 4. Быстрые операции через File
            string quickPath = "quick.txt";
            File.WriteAllLines(quickPath, new[] { "Строка 1", "Строка 2" });
            Console.WriteLine("Быстрое чтение: " + string.Join(", ", File.ReadAllLines(quickPath)));
        }
    }
}


