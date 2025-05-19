using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Application.Services.Interfaces
{
    public interface IPendingRequestService : IGenericService<PendingRequest>
    {
        /// <summary>
        /// Пагинация неподтвержденных заявок
        /// </summary>
        /// <param name="page">номер страницы</param>
        /// <param name="pageSize">размер страницы</param>
        /// <returns></returns>
        Task<PagedPage<RequestsDTO>> GetPendingRequestsDTOByPage(int page, int pageSize);
        /// <summary>
        /// Получение неподтвержденной заявки по Id
        /// </summary>
        /// <param name="id">Id неподтвержденной заявки</param>
        /// <returns></returns>
        Task<RequestsDTO?> FindById(Guid id);

        /// <summary>
        /// Обновить неподтвержденную заявку.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        Task<PendingRequest> UpdatePendingRequestData(Guid id, RequestsDTO form);


        /// <summary>
        /// Привязка заявка к персоне
        /// </summary>
        /// <param name="request"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        Task BindRequestToPerson(Guid requestId, Guid personId);

    }
}