using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "Task list : buy products, write letter to friend";
        }

        [HttpPost]
        [ActionName("Add")]
        public string AddTask()
        {
            return "Your task has been added";
        }
    }
}