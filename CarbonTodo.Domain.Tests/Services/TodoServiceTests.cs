using CarbonTodo.Domain.Exceptions;
using CarbonTodo.Domain.Models;
using CarbonTodo.Domain.Repositories;
using CarbonTodo.Domain.Services;
using Moq;
using Xunit;

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

        [Fact]
        public async Task FindAll_should_return_list()
        {
            await sut.FindAll();
            _mockTodoRepository.Verify(repo => repo.GetAll(), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task FindById_should_throw_NotFoundException_given_invalid_id()
        {
            _mockTodoRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(null as Todo).Verifiable();

            await Assert.ThrowsAsync<NotFoundEntityAppException>(async () => await sut.FindById(1));
            _mockTodoRepository.Verify(repo => repo.GetById(1));
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task FindById_should_return_expected_todo_given_valid_id()
        {
            const string title = nameof(FindById_should_return_expected_todo_given_valid_id);
            var expectedTodo = new Todo(1, title, false, 1);

            _mockTodoRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(expectedTodo).Verifiable();
            var todo = await sut.FindById(1);

            Assert.Equal(expectedTodo, todo);
            _mockTodoRepository.Verify(repo => repo.GetById(1), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }
    }
}