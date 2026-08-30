namespace To_Do.Interfaces.Common.Responses;

public record AuthenticationResponse(Guid Id, string Login, string Email, string Token);