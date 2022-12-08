using CarbonTodo.Domain.Models;
using CarbonTodo.Domain.Repositories;
using CarbonTodo.Infrastructure.Context;

namespace CarbonTodo.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        public TodoRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Task<IEnumerable<Todo>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}