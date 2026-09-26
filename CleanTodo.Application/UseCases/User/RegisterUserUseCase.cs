using CleanTodo.Application.Service;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;
using FluentValidation;

namespace CleanTodo.Application.UseCases;

public class RegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RegisterUserDto> _validator;

    public RegisterUserUseCase(IUserRepository userRepository, IValidator<RegisterUserDto> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<UserDto> Execute(RegisterUserDto registerUserDto)
    {
        var validationResult = await _validator.ValidateAsync(registerUserDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var username = registerUserDto.Username.Trim();
        if (await _userRepository.FindByUsername(username) is not null)
            throw new UsernameAlreadyExistsException(username);

        var user = new User(username, PasswordHasher.HashPassword(registerUserDto.Password));
        return new UserDto(await _userRepository.Add(user));
    }
}
