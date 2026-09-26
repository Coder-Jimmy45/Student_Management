using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Management.DTOs;
using Student_Management.Services;
using Microsoft.AspNetCore.Authorization;

namespace Student_Management.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService service, ILogger<StudentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all students");
            return Ok(await _service.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Add(StudentDTOs dto)
        {
            _logger.LogInformation("Adding student");
            await _service.Add(dto);
            return Ok("Student added");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudentDTOs dto)
        {
            _logger.LogWarning("Updating student");
            await _service.Update(id, dto);
            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogError("Deleting student");
            await _service.Delete(id);
            return Ok("Deleted");
        }
    }
}
