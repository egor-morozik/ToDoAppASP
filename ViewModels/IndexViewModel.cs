using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoApp.ViewModels
{
    public record TimeFilterModel(string Id, string Name);

    public class IndexViewModel
    {
        public IEnumerable<ToDoApp.Models.TaskItem> Tasks { get; set; } = new List<ToDoApp.Models.TaskItem>();
        public IEnumerable<TimeFilterModel> TimeFilters { get; set; } = new List<TimeFilterModel>();
        public string Status { get; set; } = "all";
        public string TimeFilter { get; set; } = "all";
        public IEnumerable<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TimeFilterOptions { get; set; } = new List<SelectListItem>();
    }
}