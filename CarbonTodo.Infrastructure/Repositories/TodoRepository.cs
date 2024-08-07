﻿using CarbonTodo.Domain.Models;
using CarbonTodo.Domain.Repositories;
using CarbonTodo.Infrastructure.Context;
using CarbonTodo.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

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
            var todos = _dbContext.Todos.OrderBy(t => t.Order);
            return ConvertToTodo(todos);
        }

        public async Task<Todo?> GetById(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);

            return todo is null ? null : ConvertToTodo(todo);
        }

        public async Task<Todo> Add(string title)
        {
            var order = GenerateOrder();
            var todoData = new TodoData(0, title, false, order);

            var todo = await _dbContext.Todos.AddAsync(todoData);
            await _dbContext.SaveChangesAsync();

            return ConvertToTodo(todo.Entity);
        }

        private int GenerateOrder()
        {
            return GetMaxOrder() + 1;
        }

        private int GetMaxOrder()
        {
            return _dbContext.Todos.Max(t => (int?)t.Order) ?? 0;
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


        public async Task<Todo> Update(int id, string title, bool completed, int order)
        {
            var todoData = await _dbContext.Todos.FindAsync(id);
            todoData.Title = title;
            todoData.Completed = completed;
            todoData.Order = order;
            var result = _dbContext.Todos.Update(todoData);
            await _dbContext.SaveChangesAsync();
            return ConvertToTodo(result.Entity);
        }

        public async Task<Todo?> GetByOrder(int order)
        {
            var todo = await _dbContext.Todos.FirstOrDefaultAsync(t => t.Order == order);
            return todo is null ? null : ConvertToTodo(todo);
        }

        public async Task UpdateCompleteAll()
        {
            await UpdateComplete(true);
        }

        public async Task UpdateComplete(Boolean completed)
        {
            foreach (var todo in _dbContext.Todos)
            {
                todo.Completed = completed;
            }

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