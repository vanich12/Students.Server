using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Models;

namespace Students.Infrastructure.Interfaces
{
    public interface IPersonRepository:IGenericRepository<Person>
    {
        /// <summary>
        /// пагинация персон
        /// </summary>
        /// <param name="page"> номер страницы</param>
        /// <param name="pageSize"> размер страницы</param>
        /// <returns></returns>
        Task<PagedPage<PersonDTO>> GetStudentsByPage(int page, int pageSize, PersonFilterDTO filters);

    }
}
