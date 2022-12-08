using CarbonTodo.Domain.Models;

namespace CarbonTodo.Domain.Services
{
    public interface ITodoService
    {
        Task<List<Todo>> FindAll();
        Task<Todo> FindById(int id);
    }
}