using System.Net;
using System.Net.Http.Json;
using CarbonTodo.Api.ApiModels.Todo;
using CarbonTodo.Infrastructure.Entities;
using Xunit;

namespace CarbonTodo.Api.Tests
{
    public class TodoControllerTests: IClassFixture<CustomWebApplicationFactory>
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
        public async Task Return_201Created_When_creating_a_todo()
        {
            const string title = nameof(Return_201Created_When_creating_a_todo);
            var todoContent = JsonContent.Create(new { Title = title });

            using var response = await _client.PostAsync("/todos", todoContent);
            var todo = await response.Content.ReadFromJsonAsync<TodoViewModel>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(new TodoViewModel(1, title, false, 1, "http://localhost/todos/1")
                , todo);
        }
        
        [Fact]
        public async Task Return_400BadRequest_when_create_given_invalid_model()
        {
            const string title = "";
            var todoContent = JsonContent.Create(new { Title = title });

            using var response = await _client.PostAsync("/todos", todoContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
    }
}