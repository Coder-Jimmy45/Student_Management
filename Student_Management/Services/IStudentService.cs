using Student_Management.DTOs;

namespace Student_Management.Services;

public interface IStudentService
{
    Task<IReadOnlyList<StudentResponse>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<StudentResponse> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<StudentResponse> CreateAsync(StudentRequest request, CancellationToken cancellationToken);
    Task<StudentResponse> UpdateAsync(int id, StudentRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
