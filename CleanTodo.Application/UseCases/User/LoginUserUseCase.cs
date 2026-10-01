using CleanTodo.Application.Service;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;
using FluentValidation;

namespace CleanTodo.Application.UseCases;

public class LoginUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<LoginUserDto> _validator;

    public LoginUserUseCase(IUserRepository userRepository, IValidator<LoginUserDto> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<UserDto> Execute(LoginUserDto loginUserDto)
    {
        var validationResult = await _validator.ValidateAsync(loginUserDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _userRepository.FindByUsername(loginUserDto.Username.Trim());
        if (user is null || !PasswordHasher.VerifyPassword(loginUserDto.Password, user.Password))
            throw new InvalidCredentialsException();

        return new UserDto(user); 
    }
}
