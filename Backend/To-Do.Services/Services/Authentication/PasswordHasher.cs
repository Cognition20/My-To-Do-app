using To_Do.Interfaces.Services.Authentication;

namespace To_Do.Services.Services.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string GeneratePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPasswordHash(string password, string hashedPassword)
    {
        return  BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}