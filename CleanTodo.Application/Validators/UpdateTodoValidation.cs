using CleanTodo.Domain.DTOS;
using FluentValidation;

namespace CleanTodo.Application.Validators;

public class UpdateTodoValidation : AbstractValidator<UpdateTodoDto>
{
    public UpdateTodoValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Le titre ne peut pas contenir seulement des espaces.")
            .MinimumLength(3)
            .MaximumLength(200);
    }
}
