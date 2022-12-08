using CarbonTodo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarbonTodo.Infrastructure.Entities
{
    public static class TodoExtensions
    {
        public static void AddTodos(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>().HasKey(todo => todo.Id);
            modelBuilder.Entity<Todo>().Property(todo => todo.Title).IsRequired();
            modelBuilder.Entity<Todo>().HasIndex(todo => todo.Order).IsUnique();
            modelBuilder.Entity<Todo>().Property(todo => todo.Completed).HasDefaultValue(false);
        }
    }
}