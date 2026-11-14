using Microsoft.EntityFrameworkCore;
using Student_Management.Data;
using Student_Management.Models;

namespace Student_Management.Repositories;

public sealed class StudentRepository(ApplicationDbContext context) : IStudentRepository
{
    public async Task<IReadOnlyList<Student>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        await context.Students.AsNoTracking().OrderBy(student => student.Id)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

    public Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        context.Students.SingleOrDefaultAsync(student => student.Id == id, cancellationToken);

    public Task<bool> EmailExistsAsync(string email, int? excludingStudentId, CancellationToken cancellationToken) =>
        context.Students.AnyAsync(student => student.Email == email &&
            (!excludingStudentId.HasValue || student.Id != excludingStudentId), cancellationToken);

    public Task AddAsync(Student student, CancellationToken cancellationToken) =>
        context.Students.AddAsync(student, cancellationToken).AsTask();

    public Task SaveChangesAsync(CancellationToken cancellationToken) => context.SaveChangesAsync(cancellationToken);
    public void Remove(Student student) => context.Students.Remove(student);
}
