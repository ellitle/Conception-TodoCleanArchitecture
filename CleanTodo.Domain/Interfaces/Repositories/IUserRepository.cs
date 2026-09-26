using CleanTodo.Domain.Entities;

namespace CleanTodo.Domain.Interfaces.Repositories;

public interface IUserRepository 
{
    Task<User> Add(User user);
    Task<User?> FindByUsername(string username);
}
