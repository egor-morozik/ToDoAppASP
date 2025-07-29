using Microsoft.AspNetCore.Mvc.ModelBinding;
using ToDoApp.Models;

namespace ToDoApp.Infrastructure
{
    public class TaskItemModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinder binder = new TaskItemModelBinder();

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(TaskItem) ? binder : null;
        }
    }
}