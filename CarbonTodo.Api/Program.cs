using CarbonTodo.Api.ActionFilters;
using CarbonTodo.Api.Extensions;
using CarbonTodo.Api.Validation;
using CarbonTodo.Domain.Repositories;
using CarbonTodo.Domain.Services;
using CarbonTodo.Infrastructure.Context;
using CarbonTodo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

string spaHost = builder.Configuration["Spa:Host"];

var connectionString = builder.Configuration.GetConnectionString("ApplicationDb");
builder.Services.AddContext(connectionString);
builder.Services.AddTransient<ITodoRepository, TodoRepository>();
builder.Services.AddTransient<ITodoService, TodoService>();
builder.Services.AddControllers(options =>
    options.Filters.Add<AppExceptionFilter>());
builder.Services.AddValidators();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(c =>
    {
        c.AllowAnyHeader();
        c.AllowAnyMethod();
        c.WithOrigins(spaHost);
    }
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi");
            options.RoutePrefix = string.Empty;
        }
    );
    app.ApplyMigrations();
}


app.UseHttpsRedirection();

app.MapControllers();
app.Run();

public partial class Program
{
}