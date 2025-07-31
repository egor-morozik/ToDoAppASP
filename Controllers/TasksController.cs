using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        [HttpGet]
        public async Task Index()
        {
            string content = @"
            <h2>Add New Task</h2>
            <form method='post'>
                <label>Description:</label><br/>
                <input name='Description' required /><br/>
                <label>Start At:</label><br/>
                <input type='datetime-local' name='StartAt' required /><br/>
                <label>End At:</label><br/>
                <input type='datetime-local' name='EndAt' required /><br/>
                <label>Is Active:</label><br/>
                <input type='checkbox' name='IsActive' value='true' checked /><br/>
                <input type='submit' value='Add Task' />
            </form>";
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync(content);
        }

        [HttpPost]
        public async Task Index(TaskItem task)
        {
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync(@$"<h2>Current task: {task.Description}</h2>
                                        <h2>Created at: {task.StartAt}</h2>
                                        <h2>End at: {task.EndAt}</h2>
                                        <h2>Is active: {task.IsActive}</h2>");
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
                            bool IsActive
                            );
    }
}