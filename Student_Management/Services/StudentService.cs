using Student_Management.DTOs;
using Student_Management.Exceptions;
using Student_Management.Models;
using Student_Management.Repositories;

namespace Student_Management.Services;

public sealed class StudentService(IStudentRepository repository) : IStudentService
{
    public async Task<IReadOnlyList<StudentResponse>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        (await repository.GetPageAsync(page, pageSize, cancellationToken)).Select(ToResponse).ToList();

    public async Task<StudentResponse> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        ToResponse(await FindStudentAsync(id, cancellationToken));

    public async Task<StudentResponse> CreateAsync(StudentRequest request, CancellationToken cancellationToken)
    {
        var email = NormalizeEmail(request.Email);
        if (await repository.EmailExistsAsync(email, null, cancellationToken))
            throw new ConflictException("A student with this email already exists.");

        var student = new Student { Name = request.Name.Trim(), Email = email, Age = request.Age,
            Course = request.Course.Trim(), CreatedAtUtc = DateTime.UtcNow };
        await repository.AddAsync(student, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return ToResponse(student);
    }

    public async Task<StudentResponse> UpdateAsync(int id, StudentRequest request, CancellationToken cancellationToken)
    {
        var student = await FindStudentAsync(id, cancellationToken);
        var email = NormalizeEmail(request.Email);
        if (await repository.EmailExistsAsync(email, id, cancellationToken))
            throw new ConflictException("A student with this email already exists.");

        student.Name = request.Name.Trim();
        student.Email = email;
        student.Age = request.Age;
        student.Course = request.Course.Trim();
        await repository.SaveChangesAsync(cancellationToken);
        return ToResponse(student);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        repository.Remove(await FindStudentAsync(id, cancellationToken));
        await repository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Student> FindStudentAsync(int id, CancellationToken cancellationToken) =>
        await repository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Student not found.");

    private static string NormalizeEmail(string email) => email.Trim().ToUpperInvariant();
    private static StudentResponse ToResponse(Student student) => new(student.Id, student.Name, student.Email, student.Age, student.Course, student.CreatedAtUtc);
}
