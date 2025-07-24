using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Controllers
{
    public record TaskItem(string Name, bool IsCompleted);

    public class TasksController : Controller
    {
        private static readonly List<TaskItem> _tasks = new List<TaskItem>
        {
            new TaskItem("Buy groceries", false),
            new TaskItem("Call a friend", true)
        };

        [HttpGet]
        public IActionResult Index(string status = "all")
        {
            StringBuilder html = new StringBuilder("<h2>Task List</h2>");

            var filteredTasks = status.ToLower() switch
            {
                "completed" => _tasks.Where(t => t.IsCompleted).ToList(),
                "notcompleted" => _tasks.Where(t => !t.IsCompleted).ToList(),
                _ => _tasks 
            };

            html.Append("<ul>");
            foreach (var task in filteredTasks)
            {
                html.Append($"<li>{task.Name} ({(task.IsCompleted ? "Completed" : "Not Completed")})</li>");
            }
            html.Append("</ul>");

            html.Append("<h3>Filter Tasks</h3>");
            html.Append("<form method='get' action='/Tasks/Index'>");
            html.Append("<select name='status'>");
            html.Append("<option value='all'>All</option>");
            html.Append("<option value='completed'>Completed</option>");
            html.Append("<option value='notcompleted'>Not Completed</option>");
            html.Append("</select>");
            html.Append("<button type='submit'>Apply</button>");
            html.Append("</form>");

            html.Append("<h3>Add Task</h3>");
            html.Append("<form method='post' action='/Tasks/Add'>");
            html.Append("<label>Task Name:</label><br />");
            html.Append("<input name='task.Name' placeholder='Enter task name' /><br />");
            html.Append("<label>Completed:</label><br />");
            html.Append("<input type='checkbox' name='task.IsCompleted' value='true' /><br />");
            html.Append("<button type='submit'>Add</button>");
            html.Append("</form>");

            return new HtmlResult(html.ToString());
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult AddTask(TaskItem task)
        {
            string? taskName = Request.Form["task.Name"];
            bool isCompleted = Request.Form["task.IsCompleted"] == "true";

            if (!string.IsNullOrEmpty(task.Name) || !string.IsNullOrEmpty(taskName))
            {
                var newTask = !string.IsNullOrEmpty(task.Name) 
                    ? task 
                    : new TaskItem(taskName ?? string.Empty, isCompleted);
                _tasks.Add(newTask);
                return new RedirectToActionResult("Index", "Tasks", null);
            }
            else
            {
                return new ContentResult
                {
                    StatusCode = 400, 
                    ContentType = "text/html;charset=utf-8",
                    Content = "<h2>Error: Task name is required</h2><a href='/Tasks/Index'>Return to Task List</a>"
                };
            }
        }
    }
}