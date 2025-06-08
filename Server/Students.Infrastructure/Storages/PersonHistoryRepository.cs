using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.DBCore.Contexts;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Filters;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Infrastructure.Storages
{
    /// <summary>
    /// История  изменений персоны
    /// </summary>
    public class PersonHistoryRepository: GenericRepository<PersonHistory>, IPersonHistoryRepository
    {
        private readonly StudentContext _ctx;
            /// <summary>
            /// Пагмнация записей в истории
            /// </summary>
            /// <param name="page"></param>
            /// <param name="pageSize"></param>
            /// <returns></returns>
        public async Task<PagedPage<PersonHistoryDTO>> GetPersonHistoryByPage(int page, int pageSize, PersonHistoryFilterDTO? filters)
        {
            IQueryable<PersonHistory> personQuery = this._ctx.PersonHistory;
            var filterItems = personQuery.ApplyFilters(filters);
            var dtoQuery = filterItems.Select(x => Mapper.PersonHistoryToPersonHistoryDTO(x).Result);
            return await PagedPage<PersonHistoryDTO>.ToPagedPage(dtoQuery, page, pageSize, x => x.ChangeDate);
        }

        public PersonHistoryRepository(StudentContext context) : base(context)
        {
            this._ctx = context;
        }
    }
}
