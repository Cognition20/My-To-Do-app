using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Services.Authentication;

namespace To_Do.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthenticationController(IAuthenticationService authenticationService) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        var result = await authenticationService.Register(registerRequest, cancellationToken);
        
        return result.Match(
            authResult => Ok(authResult),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var result = await authenticationService.Login(loginRequest, cancellationToken);
        
        return result.Match(
            authResult => Ok(authResult),
            errors => Problem(errors));
    }
}