using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace CleanTodo.Application.UseCase;

public class CreateShipUseCase
{
    private readonly IShipRepository _shipRepository;
    private readonly IValidator<CreateShipDto> _validator;
    private readonly IBrokerGateway _brokerGateway;

    public CreateShipUseCase(IShipRepository shipRepository, IValidator<CreateShipDto> validator, IBrokerGateway brokerGateway)
    {
        _shipRepository = shipRepository; 
        _validator = validator;
        _brokerGateway = brokerGateway;
    }

    public async Task<ShipDto> Execute(CreateShipDto createShipDto)
    {
        ValidationResult validationResult =
            await _validator.ValidateAsync(createShipDto);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        Ship ship = new Ship(
            createShipDto.Name,
            createShipDto.Captain,
            createShipDto.GoldCargo,
            createShipDto.CrewSize,
            _brokerGateway.CreatorId);

        Ship createdShip = await _shipRepository.Add(ship);

        return new ShipDto(createdShip);
    }
}