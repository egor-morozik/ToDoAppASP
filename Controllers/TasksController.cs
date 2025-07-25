using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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

            html.Append("<h3>Get Tasks (JSON)</h3>");
            html.Append("<a href='/Tasks/GetTasks'>View tasks in JSON</a>");

            html.Append("<h3>Redirect Demo</h3>");
            html.Append("<a href='/Tasks/RedirectToInfo'>Go to Info</a>");

            string fullHtml = $@"<!DOCTYPE html>
                                <html>
                                    <head>
                                        <title>ToDoApp</title>
                                        <meta charset=utf-8 />
                                    </head>
                                    <body>{html.ToString()}</body>
                                </html>";

            return Content(fullHtml, "text/html;charset=utf-8");
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
                return RedirectToAction("Index", "Tasks", new { status = newTask.IsCompleted ? "completed" : "notcompleted" });
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

        [HttpGet]
        public IActionResult GetTasks()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            return Json(_tasks, jsonOptions);
        }

        [HttpGet]
        public IActionResult RedirectToInfo()
        {
            return RedirectToRoute("default", new { controller = "Tasks", action = "Index", status = "all" });
        }
    }
}