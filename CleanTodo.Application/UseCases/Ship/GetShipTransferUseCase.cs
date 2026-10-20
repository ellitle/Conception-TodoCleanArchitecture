using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;

namespace CleanTodo.Application.UseCase;

public class GetShipTransferUseCase
{
    private readonly IShipTransferRepository _shipTransferRepository;

    public GetShipTransferUseCase(
        IShipTransferRepository shipTransferRepository)
    {
        _shipTransferRepository = shipTransferRepository;
    }

    public async Task<ShipTransferDto> Execute(Guid id)
    {
        var transfer = await _shipTransferRepository.FindById(id);

        if (transfer is null)
            throw new NotFoundException(id);

        return new ShipTransferDto(transfer);
    }
}