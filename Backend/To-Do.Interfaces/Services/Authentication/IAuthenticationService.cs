using ErrorOr;
using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Common.Responses;

namespace To_Do.Interfaces.Services.Authentication;

public interface IAuthenticationService
{
    Task<ErrorOr<AuthenticationResponse>> Register(RegisterRequest request,  CancellationToken cancellationToken);
    Task<ErrorOr<AuthenticationResponse>>  Login(LoginRequest request,   CancellationToken cancellationToken);
}