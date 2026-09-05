using Microsoft.EntityFrameworkCore;
using To_Do.DataAccess.ApplicationDbContext;
using To_Do.DataAccess.Models;
using To_Do.DataAccess.Repositories.Interfaces;

namespace To_Do.DataAccess.Repositories.Implementation;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await  _dbContext.Users.AddAsync(user,  cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email,  cancellationToken);
    }

    public async Task<User?> FindByLoginAsync(string login,  CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }
}