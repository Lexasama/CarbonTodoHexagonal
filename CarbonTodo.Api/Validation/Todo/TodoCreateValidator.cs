using CarbonTodo.Api.ApiModels.Todo;
using FluentValidation;

namespace CarbonTodo.Api.Validation.Todo
{
    public class TodoCreateValidator : AbstractValidator<CreateDto>
    {
        public TodoCreateValidator()
        {
            RuleFor(t => t.Title).NotEmpty();
        }
    }
}