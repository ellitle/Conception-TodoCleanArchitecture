using CleanTodo.Application.UseCase;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TodoController(
    GetAllTodosUseCase getAllUseCase,
    GetTodoUseCase getTodoUseCase,
    CreateTodoUseCase createTodoUseCase,
    UpdateTodoUseCase updateTodoUseCase,
    DeleteTodoUseCase deleteTodoUseCase,
    ToggleCompleteStatusTodoUseCase toggleTodoCompleteStatusUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetAll()
    {
        return Ok(await getAllUseCase.Execute());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoDto>> Get(Guid id)
    {
        try
        {
            return Ok(await getTodoUseCase.Execute(id));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create([FromBody] CreateTodoDto createTodoDto)
    {
        try
        {
            var todo = await createTodoUseCase.Execute(createTodoDto);
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }
        catch (ValidationException exception)
        {
            return BadRequest(ToValidationResponse(exception));
        }
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TodoDto>> Update(Guid id, [FromBody] UpdateTodoDto updateTodoDto)
    {
        try
        {
            return Ok(await updateTodoUseCase.Execute(id, updateTodoDto));
        }
        catch (ValidationException exception)
        {
            return BadRequest(ToValidationResponse(exception));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpPatch("{id:guid}/toggle")]
    public async Task<ActionResult<TodoDto>> Toggle(Guid id)
    {
        try
        {
            return Ok(await toggleTodoCompleteStatusUseCase.Execute(id));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await deleteTodoUseCase.Execute(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    private static object ToValidationResponse(ValidationException exception) => new
    {
        errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray())
    };
}
