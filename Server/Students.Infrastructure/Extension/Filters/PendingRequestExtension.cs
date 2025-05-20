using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Models;

namespace Students.Infrastructure.Extension.Filters
{
    public static class PendingRequestExtension
    {
        public static IQueryable<PendingRequest> ApplyFilters(this IQueryable<PendingRequest> query, PendingRequestFilterDTO filters) 
        {
            if (filters == null)
                return query;

            if (filters.IsArchive.HasValue)
                query = query.Where(x => x.IsArchive == false);
            

            return query;
        }
    }
}
