using CarbonTodo.Api.ApiModels.Todo;
using FluentValidation;

namespace CarbonTodo.Api.Validation.Todo
{
    public class TodoUpdateValidator : AbstractValidator<UpdateDto>
    {
        public TodoUpdateValidator()
        {
            RuleFor(t => t.Title).NotEmpty();
            RuleFor(t => t.Order).NotEmpty();
        }
    }
}