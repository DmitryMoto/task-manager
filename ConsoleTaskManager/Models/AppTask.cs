namespace ConsoleTaskManager.Models
{
    internal class AppTask
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public required string Priority { get; set; }
        public bool IsCompleted { get; set; }

        public int AuthorId { get; set; }
        public int AssigneeId { get; set; }

        public People Author { get; set; }
        public People Assignee { get; set; }
    }
}
