using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ToDoApp.Controllers
{
    public abstract class LogBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Controller: {context.Controller.GetType().Name}");

            Console.WriteLine($"Action: {context.ActionDescriptor.DisplayName}");

            if (context.ActionArguments.Any())
            {
                StringBuilder parameters = new StringBuilder("Parameters: ");
                foreach (var arg in context.ActionArguments)
                {
                    parameters.Append($"{arg.Key}={arg.Value ?? "null"}, ");
                }
                Console.WriteLine(parameters.ToString().TrimEnd(',', ' '));
            }
            else
            {
                Console.WriteLine("Parameters: None");
            }

            base.OnActionExecuting(context);
        }
    }
}