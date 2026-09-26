using CleanTodo.Application.Service;
using CleanTodo.Application.UseCases;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    LoginUserUseCase loginUserUseCase,
    RegisterUserUseCase registerUserUseCase,
    JwtService jwtService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto registerUserDto)
    {
        try
        {
            var user = await registerUserUseCase.Execute(registerUserDto);
            return Created(string.Empty, user);
        }
        catch (ValidationException exception)
        {
            return BadRequest(ToValidationResponse(exception));
        }
        catch (UsernameAlreadyExistsException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationDto>> Login([FromBody] LoginUserDto loginUserDto)
    {
        try
        {
            var user = await loginUserUseCase.Execute(loginUserDto);
            return Ok(new AuthenticationDto
            {
                Token = jwtService.GenerateToken(user.Id, user.Username),
                User = user
            });
        }
        catch (ValidationException exception)
        {
            return BadRequest(ToValidationResponse(exception));
        }
        catch (InvalidCredentialsException exception)
        {
            return Unauthorized(new { message = exception.Message });
        }
    }

    private static object ToValidationResponse(ValidationException exception) => new
    {
        errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray())
    };
}
