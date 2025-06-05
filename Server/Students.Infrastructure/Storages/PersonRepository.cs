using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Students.DBCore.Contexts;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Filters;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Infrastructure.Storages
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        private readonly StudentContext _ctx;

        public async Task<PagedPage<PersonDTO>> GetStudentsByPage(int page, int pageSize, PersonFilterDTO filters)
        {

            IQueryable<Person> personQuery = this._ctx.Persons;
            var filteredItems = personQuery.ApplyFilters(filters);
            var dtoQuery = filteredItems.Select(x => Mapper.PersonToPersonDTO(x).Result);
            return await PagedPage<PersonDTO>.ToPagedPage(dtoQuery, page, pageSize, x => x.PersonFullName);
        }

        public PersonRepository(StudentContext context) : base(context)
        {
            this._ctx = context;
        }
    }
}