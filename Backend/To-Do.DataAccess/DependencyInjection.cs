using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using To_Do.DataAccess.ApplicationDbContext;
using To_Do.DataAccess.Repositories.Implementation;
using To_Do.DataAccess.Repositories.Interfaces;

namespace To_Do.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        services.AddPersistence(configuration);
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IToDoRepository, ToDoRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        return services;
    }
}