using Student_Management.Models;

namespace Student_Management.Repositories;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, int? excludingStudentId, CancellationToken cancellationToken);
    Task AddAsync(Student student, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    void Remove(Student student);
}
