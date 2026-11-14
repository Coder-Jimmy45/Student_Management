namespace Student_Management.Models;

public sealed class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int Age { get; set; }
    public required string Course { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
