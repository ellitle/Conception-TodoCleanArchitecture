using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Interfaces.Repositories;
using CleanTodo.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanTodo.Infrastructure.Repositories;

public class ShipRepository : IShipRepository
{
    private readonly AppDbContext _context;

    public ShipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ship>> GetAll()
    {
        return await _context.Set<Ship>().ToListAsync();
    }

    public async Task<Ship?> FindById(Guid id)
    {
        return await _context.Set<Ship>()
            .Where(ship => ship.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<Ship> Add(Ship ship)
    {
        EntityEntry<Ship> newShip =
            await _context.Set<Ship>().AddAsync(ship);

        await _context.SaveChangesAsync();

        return newShip.Entity;
    }

    public async Task<Ship> BeginSail(Guid id)
    {
        var ship = await FindById(id)
            ?? throw new ShipOperationException(ShipErrorCode.NotFound);

        if (ship.Status != "docked")
        {
            throw new ShipOperationException(ShipErrorCode.AlreadySailing);
        }

        ship.Status = "sailing";
        ship.LastModified = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            await _context.Entry(ship).ReloadAsync();
            throw new ShipOperationException(ShipErrorCode.AlreadySailing);
        }

        return ship;
    }

    public async Task<Ship> Delete(Ship ship)
    {
        EntityEntry<Ship> deletedShip =
            _context.Set<Ship>().Remove(ship);

        await _context.SaveChangesAsync();

        return deletedShip.Entity;
    }

    public async Task<Ship> Update(Ship ship)
    {
        EntityEntry<Ship> updatedShip =
            _context.Set<Ship>().Update(ship);

        await _context.SaveChangesAsync();

        return updatedShip.Entity;
    }
}
