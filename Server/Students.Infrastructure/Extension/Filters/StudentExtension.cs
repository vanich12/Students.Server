using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Students.Infrastructure.DTO;
using Students.Models;
using Students.Models.ReferenceModels;

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
                query = query.AsNoTracking().Where(s => s.BirthDate == filters.BirthDate.Value);

            if (filters.ProgramEducationId.HasValue)
                query = query.AsNoTracking().Where(s =>
                    s.Groups.Any(x => x.EducationProgramId == filters.ProgramEducationId));

            if (filters.GroupId.HasValue)
                query = query.AsNoTracking().Where(s => s.Groups.Any(x => x.Id == filters.GroupId));
        

            return query;
        }
    }
}