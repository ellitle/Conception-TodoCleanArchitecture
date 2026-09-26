using CleanTodo.Domain.Entities;

namespace CleanTodo.Domain.Interfaces.Repositories;

public interface ITodoRepository
{
    Task<Todo> Add(Todo todo);
    Task<Todo> Delete(Todo todo);
    Task<Todo> Update(Todo todo);
    Task<List<Todo>> GetAll();
    Task<Todo?> FindById(Guid id);
}
