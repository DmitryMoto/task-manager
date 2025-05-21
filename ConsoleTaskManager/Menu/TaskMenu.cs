using ConsoleTaskManager.Models;
using ConsoleTaskManager.Scripts;
using ConsoleTaskManager.Services;

namespace ConsoleTaskManager.Menu
{
    internal static class TaskMenu
    {
        public static async Task ShowTaskMenu(GenericService<AppTask> appTaskService, GenericService<People> peopleService)
        {
            bool boolValue = true;

            while (boolValue)
            {
                Console.Clear();
                Console.WriteLine("Меню задач:");
                Console.WriteLine("1. Показать все задачи");
                Console.WriteLine("2. Добавить задачу");
                Console.WriteLine("3. Изменить задачу");
                Console.WriteLine("4. Удалить задачу");
                Console.WriteLine("5. Назад");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                boolValue = await(choice switch
                {
                    "1" => ShowAllTask(appTaskService),
                    "2" => AddTask(appTaskService, peopleService),
                    "3" => UpdateTask(appTaskService),
                    "4" => DeleteTask(appTaskService),
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

        private static async Task<bool> ShowAllTask(GenericService<AppTask> appTaskService)
        {
            var tasks = await appTaskService.GetAllAsync("Author", "Assignee");
            try
            {
                foreach (var task in tasks)
                {
                    Console.Clear();
                    Console.WriteLine($"{task.Id}: {task.Title} - {task.DueDate} [{task.Priority}]");
                    Console.WriteLine($"Автор: {task.Author.LastName} {task.Author.FirstName}");
                    Console.WriteLine($"Исполнитель: {task.Assignee.LastName} {task.Assignee.FirstName}");
                    Console.WriteLine($"Описание: {task.Description}");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return true;
        }

        private static async Task<bool> AddTask(GenericService<AppTask> taskService, GenericService<People> peopleService)
        {
            var authors = await peopleService.GetAllAsync();

            if (authors.Count < 1)
            {
                Console.WriteLine($"Список людей пуст. Добавьте персонал.");
                Console.WriteLine($"Нажмите любую клавишу.");
                Console.ReadLine();
                await PeopleMenu.ShowPeopleMenu(peopleService, taskService);
                return true;
            }

            Console.Write("Введите название задачи: ");
            var title = await CheckValue.ReadExistingTaskName(taskService);

            Console.Write("Введите описание задачи: ");
            var description = CheckValue.ReadString();

            Console.Write("Введите дату выполнения (дд.мм.гггг): ");
            var dueDate = CheckValue.ReadDate();

            Console.Write("Введите приоритет (Low, Medium, High): ");
            var priority = CheckValue.ReadPriority();

            Console.WriteLine("Выберите автора из списка:");
            
            foreach (var author in authors)
            {
                Console.WriteLine($"{author.Id}: {author.LastName} {author.FirstName}");
            }
            Console.Write("Введите ID автора: ");
            var authorId = await CheckValue.ReadExistingValueId(peopleService);

            Console.WriteLine("Выберите исполнителя из списка:");
            var assignees = await peopleService.GetAllAsync();
            foreach (var assignee in assignees)
            {
                Console.WriteLine($"{assignee.Id}: {assignee.LastName} {assignee.FirstName}");
            }
            Console.Write("Введите ID исполнителя: ");
            var assigneeId = await CheckValue.ReadExistingValueId(peopleService);

            var task = new AppTask
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Priority = priority,
                AuthorId = authorId,
                AssigneeId = assigneeId
            };

            await taskService.AddAsync(task);
            Console.WriteLine("Задача добавлена.");

            return true;
        }

        private static async Task<bool> UpdateTask(GenericService<AppTask> appTaskService)
        {
            Console.Write("Введите ID задачи для изменения: ");
            var id = CheckValue.ReadInt();
            var task = await appTaskService.GetByIdAsync(id);

            if (task != null)
            {
                Console.WriteLine("Изменение задачи в разработке.");
            }
            else
            {
                Console.WriteLine("Задача не найдена.");
            }

            return true;
        }

        private static async Task<bool> DeleteTask(GenericService<AppTask> appTaskService)
        {
            Console.Write("Введите ID задачи для удаления: ");
            var id = CheckValue.ReadInt();
            await appTaskService.DeleteAsync(id);
            Console.WriteLine("Задача удалена.");

            return true;
        }
    }
}
