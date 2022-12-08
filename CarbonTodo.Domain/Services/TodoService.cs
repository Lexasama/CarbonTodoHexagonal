using CarbonTodo.Domain.Models;
using CarbonTodo.Domain.Repositories;

namespace CarbonTodo.Domain.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository todoRepository)
        {
            _repository = todoRepository;
        }

        public Task<List<Todo>> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}