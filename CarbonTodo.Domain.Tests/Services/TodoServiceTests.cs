using CarbonTodo.Domain.Repositories;
using CarbonTodo.Domain.Services;
using Moq;

namespace CarbonTodo.Domain.Tests.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _mockTodoRepository;
        private readonly TodoService sut;
        

        public TodoServiceTests()
        {
            _mockTodoRepository = new Mock<ITodoRepository>();
            sut = new TodoService(_mockTodoRepository.Object);
        }

        public async Task FindAll_should_return_list()
        {
            await sut.FindAll();
            _mockTodoRepository.Verify(repo => repo.GetAll(), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }
    }
}