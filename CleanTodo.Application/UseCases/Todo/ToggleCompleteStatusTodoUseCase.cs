using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;

namespace CleanTodo.Application.UseCase;

public class ToggleCompleteStatusTodoUseCase
{
    private readonly ITodoRepository _todoRepository;
    public ToggleCompleteStatusTodoUseCase(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<TodoDto> Execute(Guid id)
    {
        Todo? todo = await _todoRepository.FindById(id);
        if (todo == null)
            throw new NotFoundException(id);

        todo.IsCompleted = !todo.IsCompleted;
        todo.CompletedOn = todo.IsCompleted ? DateTime.UtcNow : default;

        Todo updatedTodo = await _todoRepository.Update(todo);

        return new TodoDto(updatedTodo);
    }
}
