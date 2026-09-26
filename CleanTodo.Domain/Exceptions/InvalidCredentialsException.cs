namespace CleanTodo.Domain.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("Le nom d'utilisateur ou le mot de passe est invalide.")
    {
    }
}
