using CleanTodo.Domain.DTOS;
using CleanTodo.Application.UseCase;
using CleanTodo.Application.Validators;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;

using FluentValidation;
using Moq;

namespace TodoApplicationTests;

public class TodosTests
{
    private Mock<ITodoRepository> _todoRepositoryMock = null!;
    private CreateTodoUseCase _createTodoUseCase = null!;
    private GetTodoUseCase _getTodoUseCase = null!;
    private DeleteTodoUseCase _deleteTodoUseCase = null!;
    private GetAllTodosUseCase _getAllTodosUseCase = null!;
    private ToggleCompleteStatusTodoUseCase _toggleTodoCompleteStatusUseCase = null!;
    private IValidator<CreateTodoDto> _createTodoValidator = null!;
    Todo todo1 = new Todo { Id = Guid.NewGuid(), Text = "Test Todo 1" };
    Todo todo2 = new Todo { Id = Guid.NewGuid(), Text = "Test Todo 2" };


    [SetUp]
    public void Setup()
    {
        todo1 = new Todo { Id = Guid.NewGuid(), Text = "Test Todo 1" };
        todo2 = new Todo { Id = Guid.NewGuid(), Text = "Test Todo 2" };
        _createTodoValidator = new CreateTodoValidation();
        _todoRepositoryMock = new Mock<ITodoRepository>();
        _createTodoUseCase = new CreateTodoUseCase(_todoRepositoryMock.Object, _createTodoValidator);
        _getTodoUseCase = new GetTodoUseCase(_todoRepositoryMock.Object);
        _deleteTodoUseCase = new DeleteTodoUseCase(_todoRepositoryMock.Object);
        _getAllTodosUseCase = new GetAllTodosUseCase(_todoRepositoryMock.Object);
        _toggleTodoCompleteStatusUseCase = new ToggleCompleteStatusTodoUseCase(_todoRepositoryMock.Object);

        // Arrange
        _todoRepositoryMock.Setup(repo => repo.Add(It.IsAny<Todo>())).ReturnsAsync((Todo todo) => todo);
        _todoRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Todo>())).ReturnsAsync((Todo todo) => todo);
        _todoRepositoryMock.Setup(repo => repo.Update(It.IsAny<Todo>())).ReturnsAsync((Todo todo) => todo);
        _todoRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Todo> { todo1, todo2 });
        _todoRepositoryMock.Setup(repo => repo.FindById(It.Is<Guid>(id => id == todo1.Id))).ReturnsAsync(todo1);
    }

    [Test]
    public async Task CreateTodo_ShouldReturnCreatedTodo()
    {
        // Arrange
        CreateTodoDto createTodoDto = new CreateTodoDto { Title = "Test Todo" };
        // Act
        var result = await _createTodoUseCase.Execute(createTodoDto);

        // Assert
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Title, Is.EqualTo(createTodoDto.Title));
        _todoRepositoryMock.Verify(repo => repo.Add(It.Is<Todo>(todo => todo.Text == createTodoDto.Title)), Times.Once);
    }

    [Test]
    public async Task GetTodo_ShouldReturnTodo()
    {
        // Act
        var result = await _getTodoUseCase.Execute(todo1.Id);

        // Assert
        Assert.That(todo1.Id == result.Id, "Todo is returned");
        Assert.That(todo1.Text == result.Title, "Same text");
    }

    [Test]
    public async Task DeleteTodo_ShouldCallRepositoryDelete()
    {
        // Act
        await _deleteTodoUseCase.Execute(todo1.Id);

        // Assert
        _todoRepositoryMock.Verify(repo => repo.Delete(todo1), Times.Once);
    }

    [Test]
    public async Task GetAllTodos_ShouldReturnAllTodos()
    {
        // Act
        var result = await _getAllTodosUseCase.Execute();

        // Assert
        Assert.That(result.Count == 2, "Got 2 todos");
        Assert.That(todo1.Id == result[0].Id, "Both todos are returned");
        Assert.That(todo2.Id == result[1].Id, "Both todos are returned");
    }

    [Test]
    public async Task ToggleTodoCompleteStatus_ShouldCompleteTodoAndSaveIt()
    {
        // Act
        await _toggleTodoCompleteStatusUseCase.Execute(todo1.Id);

        // Assert
        Assert.That(todo1.IsCompleted, Is.True);
        Assert.That(todo1.CompletedOn, Is.Not.Null);
        _todoRepositoryMock.Verify(repo => repo.Update(todo1), Times.Once);
    }

    [Test]
    public async Task ToggleTodoCompleteStatus_ShouldReopenTodoAndClearCompletedOn()
    {
        todo1.IsCompleted = true;
        todo1.CompletedOn = DateTime.UtcNow;

        await _toggleTodoCompleteStatusUseCase.Execute(todo1.Id);

        Assert.That(todo1.IsCompleted, Is.False);
        Assert.That(todo1.CompletedOn, Is.Null);
        _todoRepositoryMock.Verify(repo => repo.Update(todo1), Times.Once);
    }

    [Test]
    public void DeletingAMissingTodo_ShouldThrowAnError()
    {
        Guid idOfAFakeTodo = Guid.NewGuid();
        // Act

        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await _deleteTodoUseCase.Execute(idOfAFakeTodo));
        _todoRepositoryMock.Verify(repo => repo.FindById(idOfAFakeTodo), Times.Once);
        _todoRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Todo>()), Times.Never);
    }
}
