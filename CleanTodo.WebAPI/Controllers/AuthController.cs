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
    private const string AuthCookieName = "CleanTodo.Auth";

    private static CookieOptions AuthCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Lax,
        Path = "/"
    };

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        try
        {
            var user = await loginUserUseCase.Execute(loginUserDto);
            var token = jwtService.GenerateToken(user.Id, user.Username);
            Response.Cookies.Append(AuthCookieName, token, AuthCookieOptions());
            Response.Headers.CacheControl = "no-store";

            return Ok(new {user});
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
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            AuthCookieName,
            AuthCookieOptions());

        Response.Headers.CacheControl = "no-store";

        return Ok(new { message = "Déconnexion réussie" });
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
