using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using ToDoApp.Models;

namespace ToDoApp.Infrastructure
{
    public static class ListHelper
    {
        public static HtmlString CreateTaskList(this IHtmlHelper html, IEnumerable<TaskItem> tasks)
        {
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("task-list");

            foreach (var task in tasks)
            {
                TagBuilder li = new TagBuilder("li");
                li.InnerHtml.Append($"{task.Name} ({task.GetStatus()}), Created At: {task.GetFormattedCreatedAt()}");
                ul.InnerHtml.AppendHtml(li);
            }

            using var writer = new StringWriter();
            ul.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}