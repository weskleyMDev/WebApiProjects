using Mapster;
using Microsoft.EntityFrameworkCore;
using StudentsApi.Api.Data;
using StudentsApi.Api.DTOs;
using StudentsApi.Api.Entities;

namespace StudentsApi.Api.Services;

public class StudentService(AppDbContext context) : IStudentService
{
    private readonly AppDbContext _context = context;

    public async Task<OutputStudentDto> CreateStudentAsync(InputStudentDto studentDto)
    {
        var student = studentDto.Adapt<Student>();
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student.Adapt<OutputStudentDto>();
    }

    public async Task<OutputStudentDto?> GetStudentByIdAsync(int id)
    {
        var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudentId == id);

        return student?.Adapt<OutputStudentDto>();
    }

    public async Task<IEnumerable<OutputStudentDto>> GetStudentsAsync()
    {
        var students = await _context.Students.AsNoTracking().ToListAsync();

        return students.Adapt<IEnumerable<OutputStudentDto>>();
    }

    public async Task<bool> RemoveStudentAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student is null)
        {
            return false;
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<OutputStudentDto?> UpdateStudentAsync(int id, InputStudentDto studentDto)
    {
        var updatedStudent = await _context.Students.FindAsync(id);

        if (updatedStudent is null)
        {
            return null;
        }

        studentDto.Adapt(updatedStudent);

        await _context.SaveChangesAsync();
        return updatedStudent.Adapt<OutputStudentDto>();
    }
}