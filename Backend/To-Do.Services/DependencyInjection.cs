using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using To_Do.Interfaces.Common.Requests;
using To_Do.Interfaces.Services;
using To_Do.Interfaces.Services.Authentication;
using To_Do.Interfaces.Services.Category;
using To_Do.Interfaces.Services.TaskManager;
using To_Do.Services.Services;
using To_Do.Services.Services.Authentication;
using To_Do.Services.Services.Category;
using To_Do.Services.Services.ToDoServices;
using To_Do.Services.Validation;
using To_Do.Services.Validation.RequestValidation;

namespace To_Do.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IToDoService, ToDoService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserId, CurrentUserId>();
        services.AddScoped<ValidationFilter>();
        services.AddScoped<IValidator<CategoryRequest>, CategoryValidation>();
        services.AddScoped<IValidator<CreateTaskRequest>, CreateTaskValidation>();
        services.AddScoped<IValidator<LoginRequest>, LoginValidation>();
        services.AddScoped<IValidator<RegisterRequest>, RegisterValidation>();
        services.AddScoped<IValidator<SearchTaskRequest>, SearchTaskValidation>();
        services.AddScoped<IValidator<UpdateTaskRequest>, UpdateTaskValidation>();
        
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Secret))
                };
                options.MapInboundClaims = false;
            });

        return services;
    }
}