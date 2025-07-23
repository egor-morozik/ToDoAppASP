using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        private static readonly List<string> _tasks = new List<string>
        {
            "Buy products",
            "Call friend"
        };

        [HttpGet]
        public async Task Index()
        {
            Response.ContentType = "text/html;charset=utf-8";
            StringBuilder html = new StringBuilder("<h2>Task list</h2>");
            html.Append("<ul>");
            foreach (var task in _tasks)
            {
                html.Append($"<li>{task}</li>");
            }
            html.Append("</ul>");
            html.Append("<form method='post' action='/Tasks/Add'>");
            html.Append("<input type='text' name='taskName' placeholder='Write task' />");
            html.Append("<button type='submit'>Add</button>");
            html.Append("</form>");

            await Response.WriteAsync(html.ToString());
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task AddTask()
        {
            Response.ContentType = "text/html;charset=utf-8";
            string ?taskName = Request.Form["taskName"];
            if (!string.IsNullOrEmpty(taskName))
            {
                _tasks.Add(taskName);
                await Response.WriteAsync("<h2>Task has been added</h2><a href='/Tasks/Index'>Return to Task list</a>");
            }
            else
            {
                Response.StatusCode = 400; 
                await Response.WriteAsync("<h2>Error: empty name</h2><a href='/Tasks/Index'>Return to Task list</a>");
            }
        }
    }
}