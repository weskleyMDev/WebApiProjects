using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentsApi.Api.Entities;

namespace StudentsApi.Api.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(student => student.StudentId);

        builder.Property(student => student.Name)
            .IsRequired()
            .HasMaxLength(80);
        
        builder.Property(student => student.Age);

        builder.Property(student => student.Email)
            .IsRequired()
            .HasMaxLength(120);
    }
}