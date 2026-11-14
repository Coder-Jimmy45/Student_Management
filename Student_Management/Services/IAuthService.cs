using Student_Management.DTOs;

namespace Student_Management.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken);

    Task<TokenResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken);
}