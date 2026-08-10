using AlunosAPI.Models.DTOs;
using AlunosAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AlunosAPI.Controllers;

/// <summary>
/// Controller responsible for managing students.
/// </summary>
/// <param name="service">Service responsible for student operations.</param>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class StudentsController(IStudentService service) : ControllerBase
{
    private readonly IStudentService _service = service;

    /// <summary>
    /// Shows a list of students.
    /// </summary>
    /// <returns>A list of students.</returns>
    /// <response code="200">Successfully found a list of students or an empty list.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OutputStudentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IAsyncEnumerable<OutputStudentDto>>> GetStudentsAsync()
    {
        var students = await _service.GetStudentsAsync();

        return Ok(students);
    }

    /// <summary>
    /// Shows a student.
    /// </summary>
    /// <param name="id">Id to locate the student.</param>
    /// <returns>The student with the corresponding id.</returns>
    /// <response code="200">Successfully found a student with your id.</response>
    /// <response code="404">Student with informed id not found.</response>
    [HttpGet("{id:int:min(1)}", Name = "GetStudentById")]
    [ProducesResponseType(typeof(OutputStudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OutputStudentDto>> GetStudentByIdAsync(int id)
    {
        var student = await _service.GetStudentByIdAsync(id);
        if (student is null)
        {
            return NotFound(new ResponseMessage
            (
                $"Student with id={id} not found!"
            ));
        }
        return Ok(student);
    }

    /// <summary>
    /// Adds a student to the database.
    /// </summary>
    /// <param name="studentDto">Data relating to the student.</param>
    /// <returns>The newly created student.</returns>
    /// <response code="400">Invalid submitted data.</response>
    /// <response code="201">Student successfully created.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OutputStudentDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<OutputStudentDto>> CreateStudentAsync(InputStudentDto studentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseMessage(
                "Invalid data!"
            ));
        }

        var newStudent = await _service.CreateStudentAsync(studentDto);

        return CreatedAtRoute("GetStudentById", new { id = newStudent.StudentId }, newStudent);
    }
}