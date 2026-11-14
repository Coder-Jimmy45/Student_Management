using Student_Management.Models;

namespace Student_Management.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken);

        Task AddAsync(
            ApplicationUser user,
            CancellationToken cancellationToken);

        Task SaveChangesAsync(
            CancellationToken cancellationToken);
    }
}
