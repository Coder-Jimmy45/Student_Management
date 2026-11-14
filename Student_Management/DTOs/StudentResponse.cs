namespace Student_Management.DTOs;

public sealed record StudentResponse(
    int Id,
    string Name,
    string Email,
    int Age,
    string Course,
    DateTime CreatedAtUtc);
