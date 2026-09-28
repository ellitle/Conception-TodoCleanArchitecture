namespace CleanTodo.Domain.DTOS;

public class AuthenticationDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = new(); 
}
