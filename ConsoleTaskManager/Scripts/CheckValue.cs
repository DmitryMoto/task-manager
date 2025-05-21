using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services;

namespace ConsoleTaskManager.Scripts
{
    internal static class CheckValue
    {
        public static async Task<string> ReadUniqueEmail(GenericService<People> peopleService)
        {
            while (true)
            {
                var email = ReadEmail();
                var existingAuthor = await peopleService.GetAllAsync();
                if (!existingAuthor.Any(a => a.Email == email))
                {
                    return email;
                }
                Console.WriteLine("Email уже используется. Пожалуйста, введите другой email.");
            }
        }

        public static async Task<int> ReadExistingValueId(GenericService<People> peopleService)
        {
            while (true)
            {
                var id = ReadInt();
                var person = await peopleService.GetByIdAsync(id);
                if (person != null)
                {
                    return id;
                }

                Console.WriteLine($"Человек с таким ID не найден. Пожалуйста, введите другой ID.");
            }
        }

        public static async Task<string> ReadExistingTaskName(GenericService<AppTask> AppTaskService)
        {
            while(true)
            {
                var taskName = ReadString();
                var existingTaskName = await AppTaskService.GetAllAsync();
                if (!existingTaskName.Any(a => a.Title == taskName))
                {
                    return taskName;
                }
                Console.WriteLine("Задача с таким названием уже существует. Пожалуйста, введите другое название.");
            }
        }

        public static string ReadPriority()
        {
            while(true)
            {
                var priority = ReadString();
                if(priority.Equals("Low") || priority.Equals("Medium") || priority.Equals("High"))
                {
                    return priority;
                }
                Console.WriteLine("Приоритет может быть: Low, Medium, High. Пожалуйста, введите значение правильно.");
            }
        }

        private static string ReadEmail()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.Contains("@"))
                {
                    return input;
                }
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректный email.");
            }
        }

        public static int ReadInt()
        {
            int result;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out result))
                {
                    return result;
                }
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите целое число.");
            }
        }

        public static string ReadString()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                Console.WriteLine("Некорректный ввод. Поле не может быть пустым.");
            }
        }

        public static DateTime ReadDate()
        {
            DateTime result;
            while (true)
            {
                if (DateTime.TryParse(Console.ReadLine(), out result))
                {
                    return result;
                }
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите дату в формате дд.мм.гггг.");
            }
        }
    }

    internal class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
