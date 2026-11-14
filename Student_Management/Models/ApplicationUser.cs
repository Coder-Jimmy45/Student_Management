

namespace Student_Management.Models;

public sealed class ApplicationUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedAtUtc { get; set; }
}
