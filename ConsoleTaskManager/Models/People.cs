using System.ComponentModel.DataAnnotations;

namespace ConsoleTaskManager.Models
{
    internal class People
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя персонала обязательно.")]
        [StringLength(100, ErrorMessage = "Имя персонала не может превышать 100 символов.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия персонала обязательно.")]
        [StringLength(200, ErrorMessage = "Фамилия персонала не может превышать 200 символов.")]
        public required string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        public required string Email { get; set; }

        public ICollection<AppTask> AuthoredTasks { get; set; } = new List<AppTask>();
        public ICollection<AppTask> AssignedTasks { get; set; } = new List<AppTask>();
    }
}
