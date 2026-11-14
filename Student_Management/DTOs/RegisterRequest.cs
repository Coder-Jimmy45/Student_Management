using System.ComponentModel.DataAnnotations;

namespace Student_Management.DTOs;

public sealed class RegisterRequest
{
    [Required, StringLength(100, MinimumLength = 3)]
    public required string Username { get; init; }

    [Required, StringLength(128, MinimumLength = 12)]
    public required string Password { get; init; }
}
