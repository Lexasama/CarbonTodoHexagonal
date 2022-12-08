﻿using CarbonTodo.Domain.Models;

namespace CarbonTodo.Domain.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAll();
        Task<Todo?> GetById(int id);
        Task<Todo> Update(int id, string title, bool completed, int order);
        Task<Todo?> GetByOrder(int order);
    }
}