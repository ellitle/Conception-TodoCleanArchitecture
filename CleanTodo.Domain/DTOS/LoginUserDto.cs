namespace CleanTodo.Domain.DTOS
{
    public class LoginUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginUserDto(){}

        public LoginUserDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
