using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanTodo.Infrastructure.Repositories;

public class ShipTransferRepository : IShipTransferRepository
{
    private readonly AppDbContext _context;

    public ShipTransferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ShipTransfer?> FindById(Guid id)
    {
        return await _context.Set<ShipTransfer>()
            .AsNoTracking()
            .SingleOrDefaultAsync(transfer => transfer.Id == id);
    }

    public Task<(Ship Ship, ShipTransfer Transfer)> BeginTransfer(
        Guid shipId,
        string destination)
    {
        throw new NotImplementedException(
            "Erreur au debut");
    }

    public Task<ShipTransfer> FinishTransfer(
        Guid transferId,
        DeliveryOutcome outcome)
    {
        throw new NotImplementedException(
            "Erreur a la fin");
    }
}