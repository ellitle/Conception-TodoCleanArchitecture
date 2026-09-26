using System;
using System.Collections.Generic;
using System.Text;

namespace CleanTodo.Application.Service
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Les anciennes donnees de demonstration contenaient un mot de passe en clair.
                return false;
            }
        }
    }
}
