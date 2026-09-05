using To_Do.Services.Validation;

namespace To_Do;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCorsConfiguration(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        return services;
    }
}
