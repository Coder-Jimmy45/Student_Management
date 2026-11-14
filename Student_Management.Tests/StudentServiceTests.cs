using Student_Management.DTOs;
using Student_Management.Exceptions;
using Student_Management.Models;
using Student_Management.Repositories;
using Student_Management.Services;
using Xunit;

namespace Student_Management.Tests;

public sealed class StudentServiceTests
{
    [Fact]
    public async Task CreateAsync_NormalizesEmailAndSavesStudent()
    {
        var repository = new FakeStudentRepository();
        var service = new StudentService(repository);

        var result = await service.CreateAsync(new StudentRequest
        {
            Name = " Ada Lovelace ", Email = " Ada@Example.com ", Age = 22, Course = " Computing "
        }, CancellationToken.None);

        Assert.Equal("ADA@EXAMPLE.COM", result.Email);
        Assert.Equal("Ada Lovelace", result.Name);
        Assert.Equal("Computing", result.Course);
        Assert.True(repository.Saved);
    }

    [Fact]
    public async Task CreateAsync_RejectsDuplicateEmail()
    {
        var repository = new FakeStudentRepository { ExistingEmail = "ADA@EXAMPLE.COM" };
        var service = new StudentService(repository);

        await Assert.ThrowsAsync<ConflictException>(() => service.CreateAsync(new StudentRequest
        {
            Name = "Ada", Email = "ada@example.com", Age = 22, Course = "Computing"
        }, CancellationToken.None));
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsForMissingStudent()
    {
        var service = new StudentService(new FakeStudentRepository());

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(404, CancellationToken.None));
    }

    private sealed class FakeStudentRepository : IStudentRepository
    {
        public string? ExistingEmail { get; init; }
        public bool Saved { get; private set; }
        public Task<IReadOnlyList<Student>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<Student>>([]);
        public Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken) => Task.FromResult<Student?>(null);
        public Task<bool> EmailExistsAsync(string email, int? excludingStudentId, CancellationToken cancellationToken) => Task.FromResult(email == ExistingEmail);
        public Task AddAsync(Student student, CancellationToken cancellationToken) { student.Id = 1; return Task.CompletedTask; }
        public Task SaveChangesAsync(CancellationToken cancellationToken) { Saved = true; return Task.CompletedTask; }
        public void Remove(Student student) { }
    }
}
