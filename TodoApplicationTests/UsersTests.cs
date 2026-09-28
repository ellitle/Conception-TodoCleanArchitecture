using CleanTodo.Application.DTOS;
using CleanTodo.Application.UseCase;
using CleanTodo.Application.UseCases;
using CleanTodo.Application.Validators;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;
using CleanTodo.Domain.UseCase;
using FluentValidation;
using Moq;

namespace TodoApplicationTests;

public class UsersTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private LoginUserUseCase _loginUserUseCase;
    private RegisterUserUseCase _registerUserUseCase;
    private IValidator<RegisterUserDto> _createUserValidator;
    Todo todo1 = new Todo { Id = Guid.NewGuid(), Text = "Test Todo 1" };
    Todo todo2 = new Todo { Id = Guid.NewGuid(), Text = "Test Todo 2" };


    [SetUp]
    public void Setup()
    {
        _createTodoValidator = new CreateTodoValidation();
        _todoRepositoryMock = new Mock<ITodoRepository>();
        _createTodoUseCase = new CreateTodoUseCase(_todoRepositoryMock.Object, _createTodoValidator);
        _getTodoUseCase = new GetTodoUseCase(_todoRepositoryMock.Object);
        _deleteTodoUseCase = new DeleteTodoUseCase(_todoRepositoryMock.Object);
        _getAllTodosUseCase = new GetAllTodosUseCase(_todoRepositoryMock.Object);
        _toggleTodoCompleteStatusUseCase = new ToggleTodoCompleteStatusUseCase(_todoRepositoryMock.Object);

        // Arrange
        _todoRepositoryMock.Setup(repo => repo.Add(It.IsAny<Todo>())).ReturnsAsync(todo1);
        _todoRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Guid>()));
        _todoRepositoryMock.Setup(repo => repo.ToggleCompleteStatus(It.IsAny<Guid>()));
        _todoRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Todo> { todo1, todo2 });
        _todoRepositoryMock.Setup(repo => repo.FindById(It.Is<Guid>(id => id == todo1.Id))).ReturnsAsync(todo1);
    }

    [Test]
    public async Task CreateUser_ShouldReturnRegisteredUser()
    {
        // Arrange
        CreateTodoDto createTodoDto = new CreateTodoDto { Title = "Test Todo" };
        // Act
        var result = await _createTodoUseCase.Execute(createTodoDto);

        // Assert
        Assert.That(todo1.Id == result.Id, "Todo is returned");
        Assert.That(todo1.Text == result.Title, "Same text");
    }
}