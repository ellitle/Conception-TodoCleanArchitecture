using CleanTodo.Domain.DTOS;
using FluentValidation;

namespace CleanTodo.Application.Validators;

public class LoginUserValidation : AbstractValidator<LoginUserDto>
{
    public LoginUserValidation()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
