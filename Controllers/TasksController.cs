using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        [HttpGet]
        public async Task Index()
        {
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync("<h2>Task list : buy products, write letter to friend</h2>");
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task AddTask()
        {
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync("<h2>Your task has been added</h2>");
        }
    }
}