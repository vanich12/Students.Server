using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Models;

namespace Students.Infrastructure.Extension.Filters
{
    public static class RequestExtension
    {
        public static IQueryable<Request> ApplyFilters(this IQueryable<Request> query,
            RequestFilterDTO filters)
        {
            if (filters.GroupId.HasValue)
                query = query.AsNoTracking().Where(x => x.GroupStudent.GroupId == filters.GroupId);

            if (filters.WithoutGroups)
                query = query.AsNoTracking().Where(x => x.GroupStudent == null);

            if (filters.HasGroup)
                query = query.AsNoTracking().Where(x => x.GroupStudent != null);
            

            return query;
        }
    }
}