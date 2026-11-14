using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management.Exceptions;

namespace Student_Management.Middleware;

public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            var (status, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Resource not found."),
                DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "The resource was modified by another request."),
                ConflictException or DbUpdateException => (StatusCodes.Status409Conflict, "The request conflicts with existing data."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };
            var details = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = environment.IsDevelopment() ? exception.Message : null,
                Instance = context.Request.Path
            };
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(details, cancellationToken: context.RequestAborted);
        }
    }
}
