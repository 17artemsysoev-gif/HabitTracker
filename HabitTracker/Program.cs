using System;
using System.Text;

namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            HabitManager manager = new HabitManager("habits.txt");
            manager.LoadHabits();

            bool isRunning = true;
            while (isRunning)
            {
                manager.PrintAll();
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1 — Добавить новую привычку");
                Console.WriteLine("2 — Отметить привычку выполненной");
                Console.WriteLine("0 — Сохранить и выйти");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Введите название привычки: ");
                        string name = Console.ReadLine();
                        manager.AddHabit(name);
                        break;

                    case "2":
                        if (manager.Habits.Count == 0)
                        {
                            Console.WriteLine("Сначала добавьте хотя бы одну привычку.");
                            break;
                        }
                        Console.Write("Введите номер привычки: ");
                        if (int.TryParse(Console.ReadLine(), out int num))
                            manager.CompleteHabit(num - 1);
                        else
                            Console.WriteLine("Введите корректное число.");
                        break;

                    case "0":
                        manager.SaveHabits();
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
        }
    }
}

