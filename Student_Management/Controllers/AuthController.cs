using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Management.DTOs;
using Student_Management.Services;

namespace Student_Management.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    // User Registration
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(
            request, cancellationToken);

        if (!result)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Username is already in use.",
                Status = StatusCodes.Status409Conflict
            });
        }

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "User registered successfully."
        });
    }

    // User Login
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(
            request, cancellationToken);

        if (response is null)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid username or password.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        return Ok(response);
    }
}
