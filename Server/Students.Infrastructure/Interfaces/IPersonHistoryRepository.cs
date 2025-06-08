using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Models;
using Students.Infrastructure.DTO.FilterDTO;

namespace Students.Infrastructure.Interfaces
{
    public interface IPersonHistoryRepository : IGenericRepository<PersonHistory>
    {
        /// <summary>
        /// Пагинация записей
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedPage<PersonHistoryDTO>> GetPersonHistoryByPage(int page, int pageSize, PersonHistoryFilterDTO? filters);
    }
}