using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;
using FluentValidation;

namespace CleanTodo.Application.UseCase;

public class UpdateTodoUseCase
{
    private readonly ITodoRepository _todoRepository;
    private readonly IValidator<UpdateTodoDto> _validator;

    public UpdateTodoUseCase(
        ITodoRepository todoRepository,
        IValidator<UpdateTodoDto> validator)
    {
        _todoRepository = todoRepository;
        _validator = validator;
    }

    public async Task<TodoDto> Execute(Guid id, UpdateTodoDto updateTodoDto)
    {
        var validationResult = await _validator.ValidateAsync(updateTodoDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var todo = await _todoRepository.FindById(id);
        if (todo is null)
            throw new NotFoundException(id);

        var wasCompleted = todo.IsCompleted;
        todo.Text = updateTodoDto.Title.Trim();
        todo.IsCompleted = updateTodoDto.IsCompleted;

        if (!wasCompleted && todo.IsCompleted)
            todo.CompletedOn = DateTime.UtcNow;
        else if (!todo.IsCompleted)
            todo.CompletedOn = default;

        return new TodoDto(await _todoRepository.Update(todo));
    }
}
