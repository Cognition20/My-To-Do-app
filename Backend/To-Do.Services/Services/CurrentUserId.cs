using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using To_Do.Interfaces.Services;

namespace To_Do.Services.Services;

public class CurrentUserId(IHttpContextAccessor httpContextAccessor) : ICurrentUserId
{


    public Guid UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId is null)
                throw new UnauthorizedAccessException();

            return Guid.Parse(userId);
        }
    }
}