using AlunosAPI.Models.DTOs;

namespace AlunosAPI.Services;

public interface IStudentService
{
    Task<IEnumerable<OutputStudentDto>> GetStudentsAsync();
    Task<OutputStudentDto?> GetStudentByIdAsync(int id);
    Task<OutputStudentDto> CreateStudentAsync(InputStudentDto studentDto);
    Task<bool> UpdateStudentAsync(int id, InputStudentDto studentDto);
    Task<bool> RemoveStudentAsync(int id);
}