using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> Add(User user)
    {
        var createdUser = await _context.Set<User>().AddAsync(user);
        await _context.SaveChangesAsync();
        return createdUser.Entity;
    }

    public async Task<User?> FindByUsername(string username)
    {
        return await _context.Set<User>()
            .SingleOrDefaultAsync(x => x.Username == username);
    }
}
