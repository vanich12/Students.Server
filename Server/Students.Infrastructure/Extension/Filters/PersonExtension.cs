using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Models;

namespace Students.Infrastructure.Extension.Filters
{
    public static class PersonExtension
    {
        /// <summary>
        /// фильтры для заявки
        /// </summary>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IQueryable<Person> ApplyFilters(this IQueryable<Person> query,
            PersonFilterDTO? filters)
        {
            // если необходимо найти ХОТЯ БЫ ОДНО совпадение
            if (filters.MatchAnyCriterion)
                return query.Where(p => filters.BirthDate.HasValue && p.BirthDate == filters.BirthDate ||
                                        filters.PhoneNumber != null && p.Phone == filters.PhoneNumber ||
                                        filters.Email !=null && p.Email == filters.Email ||
                                        filters.Name != null && p.Name == filters.Name ||
                                        filters.Family != null && p.Family == filters.Family ||
                                        filters.Patron != null && p.Patron == filters.Patron
                                        );
            
            if (filters == null)
                return query;

            // если необходимо фильтровать по всем полям в совокупе 
            if (filters.BirthDate.HasValue)
                query = query.Where(p => p.BirthDate == filters.BirthDate);

            if (filters.PhoneNumber is not null)
                query = query.Where(p => p.Phone == filters.PhoneNumber);

            if (filters.Email is not null)
                query = query.Where(p => p.Email == filters.Email);

            if (filters.Name is not null)
                query = query.Where(p => p.Name == filters.Name);

            if (filters.Family is not null)
                query = query.Where(p => p.Family == filters.Family);

            if (filters.Patron is not null)
                query = query.Where(p => p.Patron == filters.Patron);


            return query;
        }
    }
}