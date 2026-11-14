namespace Student_Management.DTOs;

public sealed record TokenResponse(string AccessToken, DateTime ExpiresAtUtc);
