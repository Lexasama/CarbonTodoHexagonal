using CarbonTodo.Api.ActionFilters;
using CarbonTodo.Api.Validation;
using CarbonTodo.Domain.Repositories;
using CarbonTodo.Domain.Services;
using CarbonTodo.Infrastructure.Context;
using CarbonTodo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApplicationDb") ?? Path.GetTempPath();
builder.Services.AddContext(connectionString);
builder.Services.AddTransient<ITodoRepository, TodoRepository>();
builder.Services.AddTransient<ITodoService, TodoService>();
builder.Services.AddControllers(options =>
    options.Filters.Add<AppExceptionFilter>());
builder.Services.AddValidators();

var app = builder.Build();

app.MapControllers();
app.Run();

public partial class Program
{
}