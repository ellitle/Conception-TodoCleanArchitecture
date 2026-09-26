namespace CleanTodo.Domain.DTOS;

public class UpdateTodoDto
{
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
