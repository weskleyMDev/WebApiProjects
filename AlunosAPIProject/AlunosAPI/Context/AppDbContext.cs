using AlunosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunosAPI.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
}