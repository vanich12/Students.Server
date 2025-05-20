using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Infrastructure.Extension.Filters
{
    public static class PendingRequestExtension
    {
        public static async Task<IQueryable<PendingRequest>> ApplyFilters(this IQueryable<PendingRequest> query,
            PendingRequestFilterDTO filters, IGenericRepository<EducationProgram> educationProgramRepository)
        {
            if (filters == null)
                return query;

            if (filters.IsArchive.HasValue)
                query = query.Where(x => x.IsArchive == filters.IsArchive);

            if (filters.ProgramEducationId.HasValue)
            {
                var programEducation = await educationProgramRepository.FindById(filters.ProgramEducationId.Value);
                if (programEducation != null)
                    query = query.Where(x => x.Education == programEducation.Name);
            }

            return query;
        }
    }
}