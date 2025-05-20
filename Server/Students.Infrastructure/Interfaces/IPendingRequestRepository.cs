using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Models;

namespace Students.Infrastructure.Interfaces
{
    public interface IPendingRequestRepository : IGenericRepository<PendingRequest>
    {
        /// <summary>
        /// Пагинация заявок.
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Пагинированные DTO заявок.</returns>
        Task<PagedPage<RequestsDTO>> GetRequestPendingByPage(int page, int pageSize,PendingRequestFilterDTO filters);
    }
}