
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Models;


namespace Students.Infrastructure.Extension.Filters
{
    public static class StudentFilterExtensions
    {
        // Метод расширения, принимающий студента
        public static IQueryable<Student> ApplyFilters(this IQueryable<Student> query, StudentFilterDto? filters)
        {
            if (filters == null)
                return query;

            if (filters.BirthDate.HasValue)
                query = query.AsNoTracking().Where(s => s.Person.BirthDate == filters.BirthDate.Value);

            if (filters.ProgramEducationId.HasValue)
                query = query.AsNoTracking().Where(s => 
                    s.Groups.Any(x => x.EducationProgramId == filters.ProgramEducationId));

            if (filters.GroupId.HasValue)
                query = query.AsNoTracking().Where(s => s.Groups.Any(x => x.Id == filters.GroupId));
        

            return query;
        }
    }
}