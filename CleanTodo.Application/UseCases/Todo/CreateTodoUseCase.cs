using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Exceptions;
using CleanTodo.Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace CleanTodo.Application.UseCase;

public class CreateTodoUseCase
{
    private readonly ITodoRepository _todoRepository;
    private readonly IValidator<CreateTodoDto> _validator;
    public CreateTodoUseCase(ITodoRepository todoRepository, IValidator<CreateTodoDto> validator)
    {
        _todoRepository = todoRepository;
        _validator = validator;
    }

    public async Task<TodoDto> Execute(CreateTodoDto TodoDto)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(TodoDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        Todo todo = await _todoRepository.Add(new Todo(TodoDto.Title));
        return new TodoDto(todo);
    }
}