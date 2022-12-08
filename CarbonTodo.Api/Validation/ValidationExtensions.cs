using CarbonTodo.Api.Validation.Todo;
using FluentValidation.AspNetCore;

namespace CarbonTodo.Api.Validation
{
    public static class ValidationExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation(config => { config.DisableDataAnnotationsValidation = true; });
            services.AddTodoValidators();
        }
    }
}