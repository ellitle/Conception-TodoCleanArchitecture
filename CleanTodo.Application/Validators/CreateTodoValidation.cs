using CleanTodo.Domain.DTOS;
using FluentValidation;

namespace CleanTodo.Application.Validators;

// Valide automatiquement CreateTodoDto quand il est créé dans le controller
// Validator ci-dessous
public class CreateTodoValidation : AbstractValidator<CreateTodoDto>
{
    public CreateTodoValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Le titre ne peut pas contenir seulement des espaces.")
            .MinimumLength(3)
            .MaximumLength(200);

    }
}
