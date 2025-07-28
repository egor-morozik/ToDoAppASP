using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public record Error(string Message);

    public class TasksController : LogBaseController
    {
        private readonly ITimeService _timeService;

        private static readonly List<TaskItem> _tasks = new List<TaskItem>
        {
            new TaskItem("Buy groceries", false, "12:00:00"),
            new TaskItem("Call a friend", true, "12:01:00")
        };

        public TasksController(ITimeService timeService)
        {
            _timeService = timeService;
        }

        [HttpGet]
        public IActionResult Index(string status = "all")
        {
            var filteredTasks = status.ToLower() switch
            {
                "completed" => _tasks.Where(t => t.IsCompleted).ToList(),
                "notcompleted" => _tasks.Where(t => !t.IsCompleted).ToList(),
                _ => _tasks 
            };

            ViewBag.Status = status;
            return View("Index", filteredTasks);
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult AddTask(TaskItem task)
        {
            string ?taskName = Request.Form["task.Name"];
            bool isCompleted = Request.Form["task.IsCompleted"] == "true";

            if (!string.IsNullOrEmpty(task.Name) || !string.IsNullOrEmpty(taskName))
            {
                var newTask = !string.IsNullOrEmpty(task.Name) 
                    ? new TaskItem(task.Name, task.IsCompleted, _timeService.Time)
                    : new TaskItem(taskName ?? string.Empty, isCompleted, _timeService.Time);
                _tasks.Add(newTask);
                return RedirectToAction("Index", "Tasks", new { status = newTask.IsCompleted ? "completed" : "notcompleted" });
            }
            else
            {
                return BadRequest(new Error("Task name is required"));
            }
        }

        [HttpGet]
        public IActionResult GetTasks(bool isAuthorized = false)
        {
            if (!isAuthorized)
            {
                return Unauthorized(new Error("You are not authorized to access tasks"));
            }

            return Ok(_tasks);
        }

        [HttpGet]
        public IActionResult FindTask(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new Error("Task name is required"));
            }

            var task = _tasks.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (task == null)
            {
                return NotFound(new Error($"Task '{name}' not found"));
            }

            return Ok(task);
        }

        [HttpGet]
        public IActionResult RedirectToInfo()
        {
            return RedirectToRoute("default", new { controller = "Tasks", action = "Index", status = "all" });
        }

        [HttpGet]
        public IActionResult DownloadTasks(string status = "all")
        {
            var filteredTasks = status.ToLower() switch
            {
                "completed" => _tasks.Where(t => t.IsCompleted).ToList(),
                "notcompleted" => _tasks.Where(t => !t.IsCompleted).ToList(),
                _ => _tasks 
            };

            StringBuilder fileContent = new StringBuilder();
            foreach (var task in filteredTasks)
            {
                fileContent.AppendLine($"Name: {task.Name}, Status: {task.GetStatus()}, Created At: {task.CreatedAt}");
            }

            byte[] fileBytes = Encoding.UTF8.GetBytes(fileContent.ToString());
            string fileName = $"tasks_{status}.txt";
            string contentType = "text/plain";

            return File(fileBytes, contentType, fileName);
        }
    }
}