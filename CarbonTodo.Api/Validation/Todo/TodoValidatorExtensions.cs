using CarbonTodo.Api.ApiModels.Todo;
using FluentValidation;

namespace CarbonTodo.Api.Validation.Todo
{
    public static class TodoValidatorExtensions
    {
        public static void AddTodoValidators(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<CreateDto>, TodoCreateValidator>();
        }
    }
}