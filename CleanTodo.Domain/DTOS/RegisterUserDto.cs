namespace CleanTodo.Domain.DTOS
{
    public class RegisterUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public RegisterUserDto(){}

        public RegisterUserDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
