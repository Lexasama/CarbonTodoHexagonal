using CarbonTodo.Domain.Models;
using CarbonTodo.Domain.Repositories;
using CarbonTodo.Infrastructure.Context;
using CarbonTodo.Infrastructure.Entities;

namespace CarbonTodo.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Todo>> GetAll()
        {
            var todos = _dbContext.Todos;
            return ConvertToTodo(todos);
        }

        public async Task<Todo?> GetById(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);

            return todo is null ? null : ConvertToTodo(todo);
        }

        public async Task DeleteAll()
        {
            _dbContext.Todos.RemoveRange(_dbContext.Todos);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCompleted()
        {
            var completedTodos = _dbContext.Todos.Where(todo => todo.Completed);
            _dbContext.Todos.RemoveRange(completedTodos);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Todo todo)
        {
            var todoData = await _dbContext.Todos.FindAsync(todo.Id);
            _dbContext.Todos.Remove(todoData);
            await _dbContext.SaveChangesAsync();
        }

        private static Todo ConvertToTodo(TodoData todoData)
        {
            return new Todo(todoData.Id, todoData.Title, todoData.Completed, todoData.Order);
        }

        private IEnumerable<Todo> ConvertToTodo(IEnumerable<TodoData> todoData)
        {
            return todoData.Select(t => new Todo(t.Id, t.Title, t.Completed, t.Order));
        }
    }
}