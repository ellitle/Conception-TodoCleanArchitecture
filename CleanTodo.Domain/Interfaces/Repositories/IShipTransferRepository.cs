using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Interfaces.Gateways;

namespace CleanTodo.Domain.Interfaces.Repositories;

public interface IShipTransferRepository
{
    Task<ShipTransfer?> FindById(Guid id);

    Task<(Ship Ship, ShipTransfer Transfer)> BeginTransfer(Guid shipId, string destination);

    Task<ShipTransfer> FinishTransfer(Guid transferId, DeliveryOutcome outcome);
}