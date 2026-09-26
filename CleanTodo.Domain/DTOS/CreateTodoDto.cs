using CleanTodo.Domain.Entities;

namespace CleanTodo.Domain.DTOS;

public class CreateTodoDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public CreateTodoDto() { }

    public CreateTodoDto(Todo todo)
    {
        Title = todo.Text;
        Date = todo.Date;
    }
}
