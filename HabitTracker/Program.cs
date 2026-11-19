using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HabitTracker
{
    public class Habit
    {
        public string Name { get; set; }
        public int Streak { get; set; }
        public bool IsCompletedToday { get; set; }

        public string ToCsvLine()
        {
            return $"{Name};{Streak};{IsCompletedToday}";
        }

        public static Habit FromCsvLine(string line)
        {
            string[] parts = line.Split(';');
            if (parts.Length != 3)
                throw new FormatException("Некорректный формат строки CSV.");

            return new Habit
            {
                Name = parts[0],
                Streak = int.Parse(parts[1]),
                IsCompletedToday = bool.Parse(parts[2])
            };
        }
    }

    public class HabitManager
    {
        private readonly string _filePath;
        public List<Habit> Habits { get; private set; } = new List<Habit>();

        public HabitManager(string filePath)
        {
            _filePath = filePath;
        }

        public void Load()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Файл habits.txt не найден. Создан новый пустой список привычек.");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(_filePath, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        try
                        {
                            Habits.Add(Habit.FromCsvLine(line));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Пропущена повреждённая строка '{line}': {ex.Message}");
                        }
                    }
                }
                Console.WriteLine($"Загружено привычек: {Habits.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }
        }

        public void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_filePath, append: false, Encoding.UTF8))
                {
                    foreach (var habit in Habits)
                    {
                        writer.WriteLine(habit.ToCsvLine());
                    }
                }
                Console.WriteLine($"Данные успешно сохранены в файл {_filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        public void AddHabit(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название привычки не может быть пустым.");
                return;
            }

            string safeName = name.Replace(";", ",");
            Habits.Add(new Habit { Name = safeName, Streak = 0, IsCompletedToday = false });
            Console.WriteLine($"Привычка «{safeName}» добавлена.");
        }

        public void CompleteHabit(int index)
        {
            if (index < 0 || index >= Habits.Count)
            {
                Console.WriteLine("Неверный номер привычки.");
                return;
            }

            var habit = Habits[index];
            if (habit.IsCompletedToday)
            {
                Console.WriteLine($"Привычка «{habit.Name}» уже отмечена выполненной сегодня.");
            }
            else
            {
                habit.IsCompletedToday = true;
                habit.Streak++;
                Console.WriteLine($"Отлично! Привычка «{habit.Name}» выполнена. Текущая серия: {habit.Streak} дн.");
            }
        }

        public void PrintAll()
        {
            Console.WriteLine("\n--- ВАШИ ПРИВЫЧКИ ---");
            if (Habits.Count == 0)
            {
                Console.WriteLine("Список пока пуст.");
                return;
            }

            for (int i = 0; i < Habits.Count; i++)
            {
                var h = Habits[i];
                string status = h.IsCompletedToday ? "[Выполнено сегодня]" : "[Не выполнено]";
                Console.WriteLine($"{i + 1}. {h.Name} | Серия: {h.Streak} дн. | {status}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            HabitManager manager = new HabitManager("habits.txt");
            manager.Load();

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
                        manager.Save();
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

