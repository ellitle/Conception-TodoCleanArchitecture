using CleanTodo.Application.Service;
using CleanTodo.Application.UseCases;
using CleanTodo.Application.Validators;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;
using FluentValidation;
using Moq;

namespace TodoApplicationTests;

public class UsersTests
{
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private LoginUserUseCase _loginUserUseCase = null!;
    private RegisterUserUseCase _registerUserUseCase = null!;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _registerUserUseCase = new RegisterUserUseCase(
            _userRepositoryMock.Object, new CreateRegisterUserValidation());
        _loginUserUseCase = new LoginUserUseCase(
            _userRepositoryMock.Object, new LoginUserValidation());
        _userRepositoryMock.Setup(repo => repo.Add(It.IsAny<User>()))
            .ReturnsAsync((User user) => user);
    }

    [Test]
    public async Task CreateUser_ShouldReturnRegisteredUserAndStoreHashedPassword()
    {
        var dto = new RegisterUserDto("  pirate  ", "Secret123!");
        User? savedUser = null;
        _userRepositoryMock.Setup(repo => repo.Add(It.IsAny<User>()))
            .Callback<User>(user => savedUser = user)
            .ReturnsAsync((User user) => user);

        var result = await _registerUserUseCase.Execute(dto);

        Assert.That(savedUser, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(savedUser!.Id));
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Username, Is.EqualTo("pirate"));
            Assert.That(savedUser.Username, Is.EqualTo("pirate"));
            Assert.That(savedUser.Password, Is.Not.EqualTo(dto.Password));
            Assert.That(PasswordHasher.VerifyPassword(dto.Password, savedUser.Password), Is.True);
        });
        _userRepositoryMock.Verify(repo => repo.FindByUsername("pirate"), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public void CreateUser_WithExistingUsername_ShouldThrowAndNotSave()
    {
        _userRepositoryMock.Setup(repo => repo.FindByUsername("pirate"))
            .ReturnsAsync(new User("pirate", "existing-hash"));

        Assert.ThrowsAsync<UsernameAlreadyExistsException>(() =>
            _registerUserUseCase.Execute(new RegisterUserDto("pirate", "Secret123!")));

        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
    }

    [Test]
    public void CreateUser_WithInvalidData_ShouldThrowBeforeAccessingRepository()
    {
        Assert.ThrowsAsync<ValidationException>(() =>
            _registerUserUseCase.Execute(new RegisterUserDto("ab", "123")));

        _userRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task LoginUser_WithValidCredentials_ShouldReturnUser()
    {
        var user = new User("pirate", PasswordHasher.HashPassword("Secret123!"));
        _userRepositoryMock.Setup(repo => repo.FindByUsername("pirate")).ReturnsAsync(user);

        var result = await _loginUserUseCase.Execute(new LoginUserDto("  pirate  ", "Secret123!"));

        Assert.That(result.Id, Is.EqualTo(user.Id));
        Assert.That(result.Username, Is.EqualTo(user.Username));
        _userRepositoryMock.Verify(repo => repo.FindByUsername("pirate"), Times.Once);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void LoginUser_WithWrongPasswordOrMissingUser_ShouldThrow(bool userExists)
    {
        User? user = userExists
            ? new User("pirate", PasswordHasher.HashPassword("Secret123!"))
            : null;
        _userRepositoryMock.Setup(repo => repo.FindByUsername("pirate")).ReturnsAsync(user);

        Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _loginUserUseCase.Execute(new LoginUserDto("pirate", "wrong-password")));

        _userRepositoryMock.Verify(repo => repo.FindByUsername("pirate"), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
    }
}
