using System.Net;
using System.Net.Http.Json;
using CarbonTodo.Api.ApiModels.Todo;
using CarbonTodo.Infrastructure.Entities;
using Xunit;

namespace CarbonTodo.Api.Tests.Controllers
{
    public class TodoControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public TodoControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Returns_a_list_with_the_right_url()
        {
            const string title = nameof(Returns_a_list_with_the_right_url);
            var todoData = new TodoData(1, title, false, 1);
            var data = new List<TodoData>
            {
                todoData
            };
            using var client = _factory.CreateClient(data);
            var todoViewModels = await client.GetFromJsonAsync<List<TodoViewModel>>("/todos");

            Assert.Equal(new List<TodoViewModel>
            {
                new(1, title, false, 1, $"http://localhost/todos/1")
            }, todoViewModels);
        }

        [Fact]
        public async Task Return_404NotFound_given_non_existing_todo_id()
        {
            using var response = await _client.GetAsync("/todos/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Return_200OK_and_todo_given_an_existing_id()
        {
            const string title = nameof(Return_200OK_and_todo_given_an_existing_id);
            var data = new List<TodoData>
            {
                new(1, title, false, 1)
            };

            using var client = _factory.CreateClient(data);
            using var response = await client.GetAsync("/todos/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Return_204NoContent_when_delete_completed()
        {
            using var response = await _client.DeleteAsync("/todos?completed=true");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Return_NoContent_when_delete_given_an_existing_todo_id()
        {
            const string title = nameof(Return_NoContent_when_delete_given_an_existing_todo_id);

            var todoData = new TodoData(1, title, false, 1);
            var data = new List<TodoData>
            {
                todoData,
                new(2, "todo2", false, 2)
            };

            using var client = _factory.CreateClient(data);

            var todosBeforeDelete = await client.GetFromJsonAsync<List<TodoViewModel>>("/todos");

            Assert.True(todosBeforeDelete?.Any(t => t.Title == title));
            Assert.Equal(new List<TodoViewModel>
            {
                new(todoData.Id, todoData.Title, todoData.Completed, todoData.Order,
                    $"http://localhost/todos/{todoData.Id}"),
                new(2, "todo2", false, 2, "http://localhost/todos/2"),
            }, todosBeforeDelete);
            using var response = await client.DeleteAsync($"/todos/{todoData.Id}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var todosAfterDelete = await client.GetFromJsonAsync<List<TodoViewModel>>("/todos");

            Assert.Equal(new List<TodoViewModel>
            {
                new(2, "todo2", false, 2, "http://localhost/todos/2")
            }, todosAfterDelete);
        }

        [Fact]
        public async Task Return_NotFound_when_Delete_given_non_existing_id()
        {
            using var response = await _client.DeleteAsync("/todos/1");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Return_NoContent_when_DeleteAll()
        {
            const string title = nameof(Return_NoContent_when_DeleteAll);
            var todoData = new TodoData(1, title, false, 1);
            var initialData = new List<TodoData>
            {
                todoData
            };
            var client = _factory.CreateClient(initialData);

            var deleteResponse = await client.DeleteAsync("/todos");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var todosViewModel = await client.GetFromJsonAsync<List<TodoViewModel>>("/todos");

            Assert.Empty(todosViewModel);
        }
    }
}