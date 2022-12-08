namespace CarbonTodo.Api.ApiModels.Todo
{
    public class CreateDto
    {
        public CreateDto(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}