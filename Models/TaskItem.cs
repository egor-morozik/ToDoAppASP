namespace ToDoApp.Models
{
    public record TaskItem(string Name, bool IsCompleted, DateTime CreatedDateTime)
    {
        public string GetStatus() => IsCompleted ? "Completed" : "Not Completed";
        public string GetFormattedCreatedAt() => CreatedDateTime.ToString("hh:mm:ss");
    }
}