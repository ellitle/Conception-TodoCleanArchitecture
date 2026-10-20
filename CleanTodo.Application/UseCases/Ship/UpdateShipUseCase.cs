using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;
using FluentValidation;

namespace CleanTodo.Application.UseCase;

public class UpdateShipUseCase
{
    private readonly IShipRepository _shipRepository;
    private readonly IValidator<UpdateShipDto> _validator;

    public UpdateShipUseCase(
        IShipRepository shipRepository,
        IValidator<UpdateShipDto> validator)
    {
        _shipRepository = shipRepository;
        _validator = validator;
    }

    public async Task<ShipDto> Execute(Guid id, UpdateShipDto updateShipDto)
    {
        var validationResult =
            await _validator.ValidateAsync(updateShipDto);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var ship = await _shipRepository.FindById(id);

        if (ship is null)
            throw new ShipOperationException(ShipErrorCode.NotFound);

        ship.Name = updateShipDto.Name.Trim();
        ship.Captain = updateShipDto.Captain.Trim();
        ship.GoldCargo = updateShipDto.GoldCargo;
        ship.CrewSize = updateShipDto.CrewSize;
        ship.LastModified = DateTime.UtcNow;

        return new ShipDto(await _shipRepository.Update(ship));
    }
}