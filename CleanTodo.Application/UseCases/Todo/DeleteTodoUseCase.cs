using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;

namespace CleanTodo.Application.UseCase;

public class DeleteTodoUseCase
{
    private readonly ITodoRepository _todoRepository;
    public DeleteTodoUseCase(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<TodoDto> Execute(Guid id)
    {
        Todo? todo = await _todoRepository.FindById(id);
        if (todo == null)
            throw new NotFoundException(id);

        Todo deletedTodo = await _todoRepository.Delete(todo);

        return new TodoDto(deletedTodo);
    }
}
