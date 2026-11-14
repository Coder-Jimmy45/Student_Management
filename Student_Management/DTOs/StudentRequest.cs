using System.ComponentModel.DataAnnotations;

namespace Student_Management.DTOs;

public sealed class StudentRequest
{
    [Required, StringLength(100)]
    public required string Name { get; init; }

    [Required, EmailAddress, StringLength(256)]
    public required string Email { get; init; }

    [Range(5, 120)]
    public int Age { get; init; }

    [Required, StringLength(100)]
    public required string Course { get; init; }
}
