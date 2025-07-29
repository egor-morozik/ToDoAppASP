using Microsoft.AspNetCore.Mvc.ModelBinding;
using ToDoApp.Models;

namespace ToDoApp.Infrastructure
{
    public class TaskItemModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var namePartValues = bindingContext.ValueProvider.GetValue("Name");
            var isCompletedPartValues = bindingContext.ValueProvider.GetValue("IsCompleted");
            var datePartValues = bindingContext.ValueProvider.GetValue("Date");
            var timePartValues = bindingContext.ValueProvider.GetValue("Time");

            var name = namePartValues.FirstValue ?? "Unnamed Task";
            var isCompleted = isCompletedPartValues.FirstValue == "true";

            DateTime.TryParse(datePartValues.FirstValue, out var parsedDateValue);
            DateTime.TryParse(timePartValues.FirstValue, out var parsedTimeValue);

            var createdDateTime = new DateTime(
                parsedDateValue.Year > 0 ? parsedDateValue.Year : DateTime.Today.Year,
                parsedDateValue.Month > 0 ? parsedDateValue.Month : DateTime.Today.Month,
                parsedDateValue.Day > 0 ? parsedDateValue.Day : DateTime.Today.Day,
                parsedTimeValue.Hour,
                parsedTimeValue.Minute,
                parsedTimeValue.Second
            );

            var task = new TaskItem(name, isCompleted, createdDateTime);
            bindingContext.Result = ModelBindingResult.Success(task);
            return Task.CompletedTask;
        }
    }
}