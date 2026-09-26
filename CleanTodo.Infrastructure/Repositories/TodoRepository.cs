using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Todo>> GetAll()
    {
        return await _context.Todos.ToListAsync();
    }

    public async Task<Todo> Add(Todo todo)
    {
        EntityEntry<Todo> newTodo = await _context.Todos.AddAsync(todo); // appelle la méthode AddAsync
        await _context.SaveChangesAsync(); // sauvegarde les changements dans la base de données
        return newTodo.Entity; // retourne l'entité ajoutée.
    }

    public async Task<Todo?> FindById(Guid id)
    {
        return await _context.Todos
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<Todo> Delete(Todo todo)
    {
        EntityEntry<Todo> deletedTodo = _context.Todos.Remove(todo); // appelle la méthode Remove
        await _context.SaveChangesAsync(); // sauvegarde les changements dans la base de données
        return deletedTodo.Entity; // retourne l'entité supprimée.
    }

    public async Task<Todo> Update(Todo todo)
    {
        EntityEntry<Todo> updatedTodo = _context.Todos.Update(todo); // appelle la méthode Update
        await _context.SaveChangesAsync(); // sauvegarde les changements dans la base de données
        return updatedTodo.Entity;
    }
}
