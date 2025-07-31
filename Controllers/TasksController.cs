using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        [HttpGet]
        public async Task Index(string taskName)
        {
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync($"<h2>Current task: {taskName}</h2>");
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task AddTask()
        {
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync("<h2>Your task has been added</h2>");
        }

        public record class TaskItem(
                            string Description,
                            DateTime StartAt,
                            DateTime EndAt,
                            bool isActive
                            );
    }
}