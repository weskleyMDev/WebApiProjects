using AlunosAPI.Context;
using AlunosAPI.Models;
using AlunosAPI.Models.DTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AlunosAPI.Services;

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

    public async Task<bool> UpdateStudentAsync(int id, InputStudentDto studentDto)
    {
        var updatedStudent = await _context.Students.FindAsync(id);

        if (updatedStudent is null)
        {
            return false;
        }

        studentDto.Adapt(updatedStudent);

        await _context.SaveChangesAsync();
        return true;
    }
}