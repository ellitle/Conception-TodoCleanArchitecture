using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;

namespace CleanTodo.Application.UseCase;

public class GetShipUseCase
{
    private readonly IShipRepository _shipRepository;

    public GetShipUseCase(IShipRepository shipRepository)
    {
        _shipRepository = shipRepository;
    }

    public async Task<ShipDto>  Execute(Guid id)
    {
        Ship? ship = await _shipRepository.FindById(id);
        if (ship == null)
        {
            throw new ShipOperationException(ShipErrorCode.NotFound);
        }
        return new ShipDto(ship);
    }
}