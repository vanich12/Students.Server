using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO.FilterDTO;

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
        Task<PagedPage<RequestsDTO>> GetPendingRequestsDTOByPage(int page, int pageSize, PendingRequestFilterDTO filters);

        /// <summary>
        /// Создание и привязка отвалидированной заявки на основе "сырой"
        /// </summary>
        /// <param name="pRequestId"> id неподтвержденной заявки</param>
        /// <param name="personId">id персоны</param>
        /// <returns></returns>
        Task<Request> CreateRequestFromPendingRequestAndPerson(Guid pRequestId, Guid personId);
        /// <summary>
        /// Создание  отвалидированной заявки на основе "сырой", также создание персоны на основе сырой заявки
        /// </summary>
        /// <param name="pRequestId">id неподтвержденной заявки</param>
        /// <returns></returns>
        Task<Request> CreateRequestFromPendingRequest(Guid pRequestId);
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