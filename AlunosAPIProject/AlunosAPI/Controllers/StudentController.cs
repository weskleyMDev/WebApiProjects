using AlunosAPI.Models.DTOs;
using AlunosAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlunosAPI.Controllers;

/// <summary>
/// Controller responsible for managing students.
/// </summary>
/// <param name="service">Service responsible for student operations.</param>
[Route("api/[controller]")]
[ApiController]
public class StudentController(IStudentService service) : ControllerBase
{
    private readonly IStudentService _service = service;

    /// <summary>
    /// Show a list of students.
    /// </summary>
    /// <returns>A list of students.</returns>
    /// <response code="200">Successfully found a list of students or an empty list.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<OutputStudentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OutputStudentDto>>> GetStudentsAsync()
    {
        var students = await _service.GetStudentsAsync();

        return Ok(students);
    }
}