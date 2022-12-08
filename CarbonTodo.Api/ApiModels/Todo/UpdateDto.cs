namespace CarbonTodo.Api.ApiModels.Todo
{
    public class UpdateDto
    {
        public string Title { get; }
        public bool Completed { get; }
        public int Order { get; }

        public UpdateDto(string title, bool completed, int order)
        {
            Title = title;
            Completed = completed;
            Order = order;
        }
    }
}