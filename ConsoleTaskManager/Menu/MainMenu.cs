using ConsoleTaskManager.Interfaces;
using ConsoleTaskManager.Models;
using ConsoleTaskManager.Services;

namespace ConsoleTaskManager.Menu
{
    internal static class MainMenu
    {
        private static ICommand _command;

        public static void SetCommand(ICommand command)
        {
            _command = command;
        }

        public static async Task ExecuteCommandAsync()
        {
            if (_command != null)
            {
                await _command.ExecuteAsync();
            }
        }
    }

    internal class TaskMenuShowCommand(GenericService<AppTask> appTaskService, GenericService<People> peopleService) : ICommand
    {
        public async Task ExecuteAsync() => await TaskMenu.ShowTaskMenu(appTaskService, peopleService);
    }

    internal class PeopleMenuShowCommand(GenericService<AppTask> appTaskService, GenericService<People> peopleService) : ICommand
    {
        public async Task ExecuteAsync() => await PeopleMenu.ShowPeopleMenu(peopleService, appTaskService);
    }
}
