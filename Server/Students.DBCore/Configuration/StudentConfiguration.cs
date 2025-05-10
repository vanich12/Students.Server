using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Students.Models;

namespace Students.DBCore.Configuration;

/// <summary>
/// Конфигурация сущности студентов.
/// </summary>
internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => x.SNILS).IsUnique();

        builder.HasMany(r => r.Requests)
            .WithOne(s => s.Student)
            .HasForeignKey(s => s.StudentId);


        builder.HasOne(p => p.Person)
            .WithOne(p => p.Student)
            .HasForeignKey<Student>(p => p.PersonId);
    }
}