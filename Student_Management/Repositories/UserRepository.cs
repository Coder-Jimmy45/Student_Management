using Microsoft.EntityFrameworkCore;
using Student_Management.Data;
using Student_Management.Models;

namespace Student_Management.Repositories
{
    public sealed class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public Task<ApplicationUser?> GetByUsernameAsync(
       string username,
       CancellationToken cancellationToken) =>
       context.Users.SingleOrDefaultAsync(
           user => user.Username == username,
           cancellationToken);

        public Task AddAsync(
            ApplicationUser user,
            CancellationToken cancellationToken) =>
            context.Users.AddAsync(user, cancellationToken).AsTask();

        public Task SaveChangesAsync(CancellationToken cancellationToken) =>
            context.SaveChangesAsync(cancellationToken);
    }
}
