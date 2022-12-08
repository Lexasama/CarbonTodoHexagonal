using CarbonTodo.Api.ActionFilters;
using CarbonTodo.Api.Validation;
using CarbonTodo.Infrastructure.Context;
using CarbonTodo.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CarbonTodo.Api.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("Test", new InMemoryDatabaseRoot())
                );
                services.AddControllers(options => options.Filters.Add<AppExceptionFilter>());
                services.AddValidators();
            });
            return base.CreateHost(builder);
        }

        public HttpClient CreateClient(IEnumerable<TodoData> data)
        {
            return new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                        var root = new InMemoryDatabaseRoot();
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseInMemoryDatabase("Test_with_data", root));
                        services.AddControllers(options => options.Filters.Add<AppExceptionFilter>());
                        services.AddValidators();

                        var sp = services.BuildServiceProvider();
                        using var scope = sp.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        db.Database.EnsureCreated();
                        db.Todos.AddRange(data);
                        db.SaveChanges();
                    });
                }).CreateClient();
        }
    }
}