using CarbonTodo.Domain.Exceptions;
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

        public async Task<List<Todo>> FindAll()
        {
            return (await _repository.GetAll()).ToList();
        }

        public async Task<Todo> FindById(int id)
        {
            var todo = await _repository.GetById(id);

            if (todo is null)
            {
                throw new NotFoundEntityAppException(nameof(todo), id);
            }

            return todo;
        }

        public async Task<Todo> Create(string title)
        {
            return await _repository.Add(title);
        }

        public async Task Delete(int id)
        {
            var todo = await FindById(id);
            await _repository.Delete(todo);
        }

        public async Task DeleteCompleted()
        {
            await _repository.DeleteCompleted();
        }

        public async Task DeleteAll()
        {
            await _repository.DeleteAll();
        }
    }
}