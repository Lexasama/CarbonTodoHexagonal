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

        [Fact]
        public async Task Create_returns_created_todo()
        {
            const string title = nameof(Create_returns_created_todo);
            var expectedTodo = new Todo(1, title, false, 1);

            _mockTodoRepository.Setup(repo => repo.Add(It.IsAny<string>()))
                .ReturnsAsync(expectedTodo).Verifiable();

            var todo = await sut.Create(title);
            Assert.Equal(expectedTodo, todo);
            _mockTodoRepository.Verify(repo => repo.Add(title), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Delete_throws_NotFoundException_given_non_existing_id()
        {
            _mockTodoRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null).Verifiable();
            await Assert.ThrowsAsync<NotFoundEntityAppException>(async () => await sut.Delete(1));

            _mockTodoRepository.Verify(repo => repo.GetById(1), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Delete_one_given_existing_id()
        {
            const string title = nameof(Delete_one_given_existing_id);
            var todoToDelete = new Todo(1, title, false, 1);
            _mockTodoRepository
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(todoToDelete).Verifiable();
            _mockTodoRepository.Setup(repo => repo.Delete(It.IsAny<Todo>()));

            await sut.Delete(todoToDelete.Id);

            _mockTodoRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _mockTodoRepository.Verify(repo => repo.Delete(todoToDelete), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteAll_deletes_all_todo()
        {
            await sut.DeleteAll();

            _mockTodoRepository.Verify(repo => repo.DeleteAll(), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteCompleted_should_delete_completed_todos()
        {
            await sut.DeleteCompleted();

            _mockTodoRepository.Verify(repo => repo.DeleteCompleted(), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Update_can_update_todo()
        {
            const string title = nameof(Update_can_update_todo);
            var expectedTodo = new Todo(1, title, false, 1);

            _mockTodoRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(expectedTodo).Verifiable();
            _mockTodoRepository.Setup(repo => repo.GetByOrder(It.IsAny<int>()))
                .ReturnsAsync(expectedTodo).Verifiable();
            const string newTitle = "new title";
            await sut.Update(expectedTodo.Id, true, newTitle, expectedTodo.Order);

            _mockTodoRepository.Verify(repo => repo.Update(expectedTodo.Id, newTitle, true, expectedTodo.Order),
                Times.Once);
            _mockTodoRepository.Verify(repo => repo.GetById(expectedTodo.Id));
            _mockTodoRepository.Verify(repo => repo.GetByOrder(expectedTodo.Order));
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Update_should_throw_NotFoundAppException_given_non_existing_Id()
        {
            const string title = nameof(Update_should_throw_NotFoundAppException_given_non_existing_Id);
            var expectedTodo = new Todo(1, title, false, 1);

            _mockTodoRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null).Verifiable();

            await Assert.ThrowsAsync<NotFoundEntityAppException>(async () =>
                await sut.Update(expectedTodo.Id, true, "new title", expectedTodo.Order));
            _mockTodoRepository.Verify(repo => repo.GetById(expectedTodo.Id));
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Update_should_throw_ConflictingOrderAppException_given_existing_order()
        {
            const string title = nameof(Update_should_throw_ConflictingOrderAppException_given_existing_order);
            var expectedTodo = new Todo(1, title, false, 1);
            _mockTodoRepository.Setup(repo => repo.GetById(expectedTodo.Id))
                .ReturnsAsync(expectedTodo).Verifiable();
            _mockTodoRepository.Setup(repo => repo.GetByOrder(It.IsAny<int>()))
                .ReturnsAsync(new Todo(2, "todo", false, expectedTodo.Order)).Verifiable();

            await Assert.ThrowsAsync<ConflictingOrderAppException>(async () =>
                await sut.Update(expectedTodo.Id, true, "new title", 2));
            _mockTodoRepository.Verify(repo => repo.GetById(expectedTodo.Id), Times.Once);
            _mockTodoRepository.Verify(repo => repo.GetByOrder(It.IsAny<int>()), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task FindByOrder_should_return_todo()
        {
            const string title = nameof(FindByOrder_should_return_todo);
            var expectedTodo = new Todo(1, title, false, 1);

            _mockTodoRepository.Setup(repo => repo.GetByOrder(It.IsAny<int>()))
                .ReturnsAsync(expectedTodo).Verifiable();

            await sut.FindByOrder(expectedTodo.Order);

            _mockTodoRepository.Verify(repo => repo.GetByOrder(expectedTodo.Id));
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task FindByOrder_Should_throw_NotFoundEntityAppException_given_non_existing_order()
        {
            const int order = 1;

            await Assert.ThrowsAsync<NotFoundEntityAppException>(async () => await sut.FindByOrder(order));
            _mockTodoRepository.Verify(repo => repo.GetByOrder(order));
            _mockTodoRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CompleteAll_should_update_all_todos()
        {
            await sut.CompleteAll();
            _mockTodoRepository.Verify(repo => repo.UpdateCompleteAll(), Times.Once);
            _mockTodoRepository.VerifyNoOtherCalls();
        }
    }
}