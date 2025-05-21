using ConsoleTaskManager.Models;
using ConsoleTaskManager.Scripts;
using ConsoleTaskManager.Services;
using System.ComponentModel.DataAnnotations;
using ValidationException = ConsoleTaskManager.Scripts.ValidationException;

namespace ConsoleTaskManager.Menu
{
    internal static class PeopleMenu
    {
        public static async Task ShowPeopleMenu(GenericService<People> peopleService, GenericService<AppTask> appTaskService) 
        {
            bool boolValue = true;

            while (boolValue)
            {
                Console.Clear();
                Console.WriteLine($"Меню персонала:");
                Console.WriteLine($"1. Показать весь персонал");
                Console.WriteLine($"2. Добавить персонал");
                Console.WriteLine($"3. Изменить данные об персонале");
                Console.WriteLine($"4. Удалить персонал");
                Console.WriteLine("5. Назад");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                boolValue = await (choice switch
                {
                    "1" => ShowAllPeople(peopleService, appTaskService),
                    "2" => AddPeopleValidationAsync(peopleService),
                    "3" => UpdatePeople(peopleService),
                    "4" => DeletePeople(peopleService),
                    "5" => Task.FromResult(false),
                    _ => Task.Run(() =>
                    {
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        return true;
                    })
                });

                Console.WriteLine("Нажмите Enter для продолжения...");
                Console.ReadLine();
            }
        }

        private static async Task<bool> ShowAllPeople(GenericService<People> peopleService, GenericService<AppTask> appTaskService) 
        {
            var people = await peopleService.GetAllAsync();
            var tasks = await appTaskService.GetAllAsync();
            if(people.Count != 0)
            {
                Console.WriteLine($"Список персонала:");

                foreach (var person in people)
                {
                    Console.WriteLine($"{person.Id}: {person.LastName} {person.FirstName} - {person.Email}");
                    Console.WriteLine($"   Является автором задач:");

                    var authorTasks = tasks.Where(t => t.AuthorId == person.Id).ToList();
                    if (authorTasks.Count != 0)
                    {
                        foreach (var authorTask in authorTasks)
                        {
                            Console.WriteLine($"   - {authorTask.Title}  ({authorTask.Priority})");
                        }
                    }
                    else Console.WriteLine("   - Нет задач, где человек является автором.");
                    Console.WriteLine($"   Является исполнителем задач:");

                    var assigneeTasks = tasks.Where(t => t.AssigneeId == person.Id).ToList();
                    if (assigneeTasks.Count != 0)
                    {
                        foreach (var assigneeTask in assigneeTasks)
                        {
                            Console.WriteLine($"   - {assigneeTask.Title}  ({assigneeTask.Priority})");
                        }
                    }
                    else Console.WriteLine("   - Нет задач, где человек является исполнителем.");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"Список персонала пуст.");
            }

            return true;
        }

        private static async Task AddPeopleValidation(GenericService<People> peopleService, People people)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(people);

            if (!Validator.TryValidateObject(people, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
                return;
            }

            await peopleService.AddAsync(people);
            Console.WriteLine("Персонал добавлен");
        }

        private static async Task<bool> AddPeopleValidationAsync(GenericService<People> peopleService)
        {
            Console.WriteLine("Введите фамилию человека: ");
            var lastName = Console.ReadLine();

            Console.WriteLine("Введите имя человека: ");
            var firstName = Console.ReadLine();

            Console.WriteLine("Введите email человека: ");
            var email = Console.ReadLine();

            var person = new People
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };

            try
            {
                await AddPeopleValidation(peopleService, person);
                Console.WriteLine("Автор успешно добавлен.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Ошибка валидации: {ex.Message}");
            }

            return true;
        }

        private static async Task<bool> AddPeople(GenericService<People> peopleService)
        {
            Console.WriteLine("Введите фамилию человека: ");
            var lastName = CheckValue.ReadString();

            Console.WriteLine("Введите имя человека: ");
            var firstName = CheckValue.ReadString();

            Console.WriteLine("Введите email человека: ");
            var email = await CheckValue.ReadUniqueEmail(peopleService);

            var person = new People
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };

            await peopleService.AddAsync(person);
            Console.WriteLine("Персонал добавлен");

            return true;
        }

        private static async Task<bool> UpdatePeople(GenericService<People> peopleService)
        {
            Console.Write("Введите ID человека для изменения: ");
            var id = CheckValue.ReadInt();
            var person = await peopleService.GetByIdAsync(id);

            if (person != null)
            {
                Console.WriteLine("Изменение персонала в разработке.");
            }
            else
            {
                Console.WriteLine("Персонал не найден.");
            }

            return true;
        }

        private static async Task<bool> DeletePeople(GenericService<People> peopleService)
        {
            Console.Write("Введите ID человека для удаления: ");
            var id = CheckValue.ReadInt();
            await peopleService.DeleteAsync(id);
            Console.WriteLine("Персонал удален.");

            return true;
        }
    }
}
