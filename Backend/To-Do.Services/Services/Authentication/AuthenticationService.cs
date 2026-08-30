using To_Do.DataAccess.Common.Errors;
using To_Do.DataAccess.Models;
using To_Do.DataAccess.Repositories.Interfaces;
using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Common.Responses;
using To_Do.Interfaces.Services.Authentication;
using ErrorOr;

namespace To_Do.Services.Services.Authentication;

public class AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator) : IAuthenticationService
{
    public async Task<ErrorOr<AuthenticationResponse>> Register(RegisterRequest request)
    {
        var userByLogin   = await userRepository.FindByLoginAsync(request.Login);
        var userByEmail  = await userRepository.FindByEmailAsync(request.Email);

        if (userByLogin  is not null)
            return Errors.User.DuplicateLogin(request.Login);
        
        if (userByEmail  is not null)
            return Errors.User.DuplicateEmail(request.Email);
        
        var passwordHash = passwordHasher.GeneratePasswordHash(request.Password);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = request.Login,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        await userRepository.AddAsync(user);

        var token = jwtTokenGenerator.GenerateJwtToken(
            user.Id,
            user.Login,
            user.Email);

        return new AuthenticationResponse(
            user.Id,
            user.Login,
            user.Email,
            token);
    }

    public async Task<ErrorOr<AuthenticationResponse>> Login(LoginRequest request)
    {
        var user = await userRepository.FindByEmailAsync(request.Email);

        if (user is null)
            return Errors.Authentication.InvalidCredentials;

        var isValidPassword = passwordHasher.VerifyPasswordHash(request.Password, user.PasswordHash);
        
        if (!isValidPassword)
            return Errors.Authentication.InvalidCredentials;
        
        var token = jwtTokenGenerator.GenerateJwtToken(
            user.Id,
            user.Login,
            user.Email);
        
        return new AuthenticationResponse(
            user.Id,
            user.Login,
            user.Email,
            token);
    }
}