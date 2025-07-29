using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Controllers
{
    public record Error(string Message);

    public class TasksController : LogBaseController
    {
        private readonly ITimeService _timeService;

        private static readonly List<TaskItem> _tasks = new List<TaskItem>
        {
            new TaskItem("Buy groceries", false, new DateTime(2025, 7, 28, 12, 0, 0)),
            new TaskItem("Call a friend", true, new DateTime(2025, 7, 27, 12, 1, 0))
        };

        public TasksController(ITimeService timeService)
        {
            _timeService = timeService;
        }

        [HttpGet]
        public IActionResult Index(string status = "all", string timeFilter = "all")
        {
            var filteredTasks = _tasks.AsEnumerable();
            if (status.ToLower() != "all")
            {
                filteredTasks = status.ToLower() == "completed" 
                    ? filteredTasks.Where(t => t.IsCompleted) 
                    : filteredTasks.Where(t => !t.IsCompleted);
            }
            if (timeFilter.ToLower() == "today")
            {
                filteredTasks = filteredTasks.Where(t => t.CreatedDateTime.Date == DateTime.Today);
            }

            var timeFilters = new List<TimeFilterModel>
            {
                new TimeFilterModel("all", "All"),
                new TimeFilterModel("today", "Today")
            };

            var viewModel = new IndexViewModel
            {
                Tasks = filteredTasks,
                TimeFilters = timeFilters,
                Status = status,
                TimeFilter = timeFilter
            };

            return View("Index", viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            _tasks.Add(task);
            return RedirectToAction("Index", new { status = task.IsCompleted ? "completed" : "notcompleted", timeFilter = "today" });
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
                    ? new TaskItem(task.Name, task.IsCompleted, DateTime.Now)
                    : new TaskItem(taskName ?? string.Empty, isCompleted, DateTime.Now);
                _tasks.Add(newTask);
                return RedirectToAction("Index", "Tasks", new { status = newTask.IsCompleted ? "completed" : "notcompleted", timeFilter = "today" });
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
            return RedirectToRoute("default", new { controller = "Tasks", action = "Index", status = "all", timeFilter = "all" });
        }

        [HttpGet]
        public IActionResult DownloadTasks(string status = "all", string timeFilter = "all")
        {
            var filteredTasks = _tasks.AsEnumerable();
            if (status.ToLower() != "all")
            {
                filteredTasks = status.ToLower() == "completed" 
                    ? filteredTasks.Where(t => t.IsCompleted) 
                    : filteredTasks.Where(t => !t.IsCompleted);
            }
            if (timeFilter.ToLower() == "today")
            {
                filteredTasks = filteredTasks.Where(t => t.CreatedDateTime.Date == DateTime.Today);
            }

            StringBuilder fileContent = new StringBuilder();
            foreach (var task in filteredTasks)
            {
                fileContent.AppendLine($"Name: {task.Name}, Status: {task.GetStatus()}, Created At: {task.GetFormattedCreatedAt()}");
            }

            byte[] fileBytes = Encoding.UTF8.GetBytes(fileContent.ToString());
            string fileName = $"tasks_{status}_{timeFilter}.txt";
            string contentType = "text/plain";

            return File(fileBytes, contentType, fileName);
        }
    }
}