using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Models;

namespace Students.Infrastructure.Extension.Filters
{
    public static class PersonHistoryExtension
    {
        public static IQueryable<PersonHistory> ApplyFilters(this IQueryable<PersonHistory> query,
            PersonHistoryFilterDTO? filters)
        {
            if (query is null)
                return null;

            if (filters is null)
                return query;

            if (filters.PersonId.HasValue)
                query = query.Where(x => x.PersonId == filters.PersonId);

            return query;
        }
    }
}