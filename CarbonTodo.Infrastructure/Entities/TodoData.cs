namespace CarbonTodo.Infrastructure.Entities
{
    public class TodoData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Order { get; set; }

        public TodoData(int id, string title, bool completed, int order)
        {
            Id = id;
            Title = title;
            Completed = completed;
            Order = order;
        }
    }
}