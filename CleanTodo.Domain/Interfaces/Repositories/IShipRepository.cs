using CleanTodo.Domain.Entities;

namespace CleanTodo.Domain.Interfaces.Repositories;

public interface IShipRepository
{
    Task<Ship> Add(Ship ship);
    Task<Ship> Delete(Ship ship);
    Task<Ship> Update(Ship ship);
    Task<List<Ship>> GetAll();
    Task<Ship?> FindById(Guid id);
    Task<Ship> BeginSail(Guid id);
}
