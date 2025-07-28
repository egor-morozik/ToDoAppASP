namespace ToDoApp.Models
{
    public record TaskItem(string Name, bool IsCompleted, string CreatedAt)
    {
        public string GetStatus() => IsCompleted ? "Completed" : "Not Completed";
    }
}