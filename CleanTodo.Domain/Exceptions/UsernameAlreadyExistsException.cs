namespace CleanTodo.Domain.Exceptions;

public class UsernameAlreadyExistsException : Exception
{
    public UsernameAlreadyExistsException(string username)
        : base($"Le nom d'utilisateur '{username}' existe deja.")
    {
    }
}
