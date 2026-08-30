namespace To_Do.Interfaces.Services.Authentication;

public interface IPasswordHasher
{
    string GeneratePasswordHash(string password);
    bool VerifyPasswordHash(string password, string hashedPassword);
}