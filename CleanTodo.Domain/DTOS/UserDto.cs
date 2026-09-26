using CleanTodo.Domain.Entities;

namespace CleanTodo.Domain.DTOS;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;

    public UserDto() { } 


    // Devrait être fait dans Mapping -> automapper.
    public UserDto(User user)
    {
        Id = user.Id;
        Username = user.Username;
    }
}
