using CarbonTodo.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarbonTodo.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TodoData> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddTodos();
        }
    }
}