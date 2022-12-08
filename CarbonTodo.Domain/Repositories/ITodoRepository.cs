using CarbonTodo.Domain.Models;

namespace CarbonTodo.Domain.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAll();

        Task<Todo?> GetById(int id);

        Task DeleteAll();

        Task DeleteCompleted();

        Task Delete(Todo todo);
    }
}