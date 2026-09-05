using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindByEmailAsync(string email,  CancellationToken cancellationToken);
    Task<User?> FindByLoginAsync(string login,   CancellationToken cancellationToken);
}