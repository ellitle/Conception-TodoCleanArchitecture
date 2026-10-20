using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace CleanTodo.Application.UseCase;

public class DockShipUseCase
{
    private readonly IShipRepository _shipRepository;
    private readonly IValidator<BrokerShipDto> _validator;

    public DockShipUseCase(
        IShipRepository shipRepository,
        IValidator<BrokerShipDto> validator)
    {
        _shipRepository = shipRepository;
        _validator = validator;
    }

    public async Task<ShipDto> Execute(BrokerShipDto brokerShipDto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(brokerShipDto);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        Ship ship = new Ship(brokerShipDto.Name, brokerShipDto.Captain, brokerShipDto.GoldCargo, brokerShipDto.CrewSize, brokerShipDto.CreatedBy);

        ship.CreatedAt = brokerShipDto.CreatedAt.UtcDateTime;
        ship.LastModified = brokerShipDto.LastModified.UtcDateTime;

        Ship createdShip = await _shipRepository.Add(ship);

        return new ShipDto(createdShip);
    }
}