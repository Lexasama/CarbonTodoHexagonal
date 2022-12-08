using CarbonTodo.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApplicationDb") ?? Path.GetTempPath();
builder.Services.AddContext(connectionString);

var app = builder.Build();
app.Run();