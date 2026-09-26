using CleanTodo.Domain.DTOS;
using FluentValidation;

namespace CleanTodo.Application.Validators;

// Valide automatiquement CreateTodoDto quand il est créé dans le controller
// Validator ci-dessous
public class CreateRegisterUserValidation : AbstractValidator<RegisterUserDto>
{
    public CreateRegisterUserValidation()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Must(username => !string.IsNullOrWhiteSpace(username))
            .WithMessage("Le nom d'utilisateur ne peut pas contenir seulement des espaces.")
            .MinimumLength(3)
            .MaximumLength(200);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(200);
    }
}
