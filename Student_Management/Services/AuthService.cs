using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Student_Management.DTOs;
using Student_Management.Models;
using Student_Management.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Student_Management.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IPasswordHasher<ApplicationUser> passwordHasher,
    IConfiguration configuration) : IAuthService
{
    // Registration
    public async Task<bool> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var username = NormalizeUsername(request.Username);

        // Check whether username already exists
        var existingUser = await userRepository.GetByUsernameAsync(
            username, cancellationToken);

        if (existingUser is not null)
            return false;

        // Create normal User account
        var user = new ApplicationUser
        {
            Username = username,
            PasswordHash = string.Empty,
            Role = "User",
            CreatedAtUtc = DateTime.UtcNow
        };

        // Hash password
        user.PasswordHash = passwordHasher.HashPassword(
            user, request.Password);

        // Save user
        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    // Login
    public async Task<TokenResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var username = NormalizeUsername(request.Username);

        var user = await userRepository.GetByUsernameAsync(
            username, cancellationToken);

        if (user is null)
            return null;

        // Verify password
        var result = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        // Upgrade password hash when required
        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = passwordHasher.HashPassword(
                user, request.Password);

            await userRepository.SaveChangesAsync(cancellationToken);
        }

        // Generate JWT
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(60);

        var token = CreateToken(user, expiresAtUtc);

        return new TokenResponse(token, expiresAtUtc);
    }

    // Generate JWT
    private string CreateToken(
        ApplicationUser user,
        DateTime expiresAtUtc)
    {
        var jwtKey = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key is missing.");

        var issuer = configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer is missing.");

        var audience = configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT Audience is missing.");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string NormalizeUsername(string username) =>
        username.Trim().ToUpperInvariant();
}