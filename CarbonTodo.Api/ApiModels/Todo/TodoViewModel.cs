namespace CarbonTodo.Api.ApiModels.Todo
{
    public record TodoViewModel(int Id, string Title, bool Completed, int Order, string Url)
    {
        public static TodoViewModel From(Domain.Models.Todo todo, string url)
        {
            return new TodoViewModel(todo.Id, todo.Title, todo.Completed, todo.Order, url);
        }
    }
}