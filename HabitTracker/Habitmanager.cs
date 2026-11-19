
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HabitTracker
{
    public class HabitManager
    {
        private readonly string _filePath;
        public List<Habit> Habits { get; private set; } = new List<Habit>();

        public HabitManager(string filePath)
        {
            _filePath = filePath;
        }

        public List<Habit> LoadHabits()
        {
            Habits.Clear();

            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Файл habits.txt не найден. Создан новый пустой список привычек.");
                return Habits;
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

            return Habits;
        }

        public void SaveHabits()
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
}
