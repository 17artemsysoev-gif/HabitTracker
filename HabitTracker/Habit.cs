using System;

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
}


