using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByLoginAsync(string login);
}