using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Management.DTOs;
using Student_Management.Services;

namespace Student_Management.Controllers;

[Authorize (Roles = "User")]
[ApiController]
[Route("api/students")]
public sealed class StudentController(IStudentService service, ILogger<StudentController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StudentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StudentResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || pageSize is < 1 or > 100)
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { ["pagination"] = ["page must be at least 1 and pageSize must be between 1 and 100."] }));

        return Ok(await service.GetPageAsync(page, pageSize, cancellationToken));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentResponse>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await service.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<StudentResponse>> Create(StudentRequest request, CancellationToken cancellationToken)
    {
        var student = await service.CreateAsync(request, cancellationToken);
        logger.LogInformation("Created student {StudentId}", student.Id);
        return CreatedAtAction(nameof(GetById), new { student.Id }, student);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentResponse>> Update(int id, StudentRequest request, CancellationToken cancellationToken)
    {
        var student = await service.UpdateAsync(id, request, cancellationToken);
        logger.LogInformation("Updated student {StudentId}", student.Id);
        return Ok(student);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        logger.LogInformation("Deleted student {StudentId}", id);
        return NoContent();
    }
}
