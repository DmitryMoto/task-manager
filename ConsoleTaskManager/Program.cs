using ConsoleTaskManager.Data;
using ConsoleTaskManager.Interfaces;
using ConsoleTaskManager.Menu;
using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var context = new AppDbContext();
        var taskService = new GenericService<AppTask>(context);
        var peopleService = new GenericService<People>(context);

        var command = new Dictionary<string, ICommand>
        {
            { "1", new TaskMenuShowCommand(taskService, peopleService) },
            { "2", new PeopleMenuShowCommand(taskService, peopleService) }
        };

        bool boolValue = true;

        while (boolValue)
        {
            Console.Clear();
            Console.WriteLine("1. Меню задач");
            Console.WriteLine("2. Меню персонала");
            Console.WriteLine("3. Выйти");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();

            boolValue = choice == "3" ? false : true;

            if (command.ContainsKey(choice))
            {
                MainMenu.SetCommand(command[choice]);
                await MainMenu.ExecuteCommandAsync();
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }


        }
    }
}