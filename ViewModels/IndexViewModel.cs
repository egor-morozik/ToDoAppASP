namespace ToDoApp.ViewModels
{
    public record TimeFilterModel(string Id, string Name);

    public class IndexViewModel
    {
        public IEnumerable<ToDoApp.Models.TaskItem> Tasks { get; set; } = new List<ToDoApp.Models.TaskItem>();
        public IEnumerable<TimeFilterModel> TimeFilters { get; set; } = new List<TimeFilterModel>();
        public string Status { get; set; } = "all";
        public string TimeFilter { get; set; } = "all";
    }
}