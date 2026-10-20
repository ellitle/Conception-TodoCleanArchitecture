using CleanTodo.Domain.DTOS;
using FluentValidation;

namespace CleanTodo.Application.Validators;

public class BrokerShipValidation : AbstractValidator<BrokerShipDto>
{
    public BrokerShipValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Le nom ne peut pas contenir seulement des espaces.")
            .Length(2, 100)
            .Must(name => name is not null && name.Trim().Length >= 2);

        RuleFor(x => x.Captain)
            .NotEmpty()
            .Must(captain => !string.IsNullOrWhiteSpace(captain))
            .WithMessage("Le capitaine ne peut pas contenir seulement des espaces.")
            .Length(2, 50)
            .Must(captain => captain is not null && captain.Trim().Length >= 2);

        RuleFor(x => x.GoldCargo)
            .InclusiveBetween(0m, 1_000_000m);

        RuleFor(x => x.CrewSize)
            .InclusiveBetween(1, 500);

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .Must(createdBy => !string.IsNullOrWhiteSpace(createdBy))
            .WithMessage("Le créateur ne peut pas contenir seulement des espaces.")
            .MaximumLength(200);

        RuleFor(x => x.CreatedAt)
            .NotEmpty();

        RuleFor(x => x.LastModified)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.CreatedAt);

        RuleFor(x => x.Status)
            .Equal("sailing");
    }
}
