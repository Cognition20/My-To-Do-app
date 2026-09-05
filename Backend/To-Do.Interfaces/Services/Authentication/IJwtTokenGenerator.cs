namespace To_Do.Interfaces.Services.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(Guid userId, string login, string email);
}