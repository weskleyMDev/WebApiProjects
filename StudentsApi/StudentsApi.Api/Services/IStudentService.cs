using StudentsApi.Api.DTOs;

namespace StudentsApi.Api.Services;

public interface IStudentService
{
    Task<IEnumerable<OutputStudentDto>> GetStudentsAsync();
    Task<OutputStudentDto?> GetStudentByIdAsync(int id);
    Task<OutputStudentDto> CreateStudentAsync(InputStudentDto studentDto);
    Task<OutputStudentDto?> UpdateStudentAsync(int id, InputStudentDto studentDto);
    Task<bool> RemoveStudentAsync(int id);
}