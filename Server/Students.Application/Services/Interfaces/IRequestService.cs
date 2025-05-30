using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Models;

namespace Students.Application.Services.Interfaces
{
    public interface IRequestService : IGenericService<Request>
    {
        /// <summary>
        /// Добавление приказа в заявку
        /// </summary>
        /// <param name="requestId">Идентификатор заявки</param>
        /// <param name="order">Приказ</param>
        /// <returns></returns>
        Task<Guid?> AddOrderToRequest(Guid requestId, Order order);

        /// <summary>
        /// Список заявок, которые подавал студент
        /// </summary>
        /// <param name="studentId">Идентификатор студента</param>
        /// <returns>Список заявок</returns>
        Task<IEnumerable<RequestsDTO>?> GetListRequestsOfStudentExists(Guid studentId, RequestFilterDTO? filter);

        /// <summary>
        /// Создание заявки 
        /// </summary>
        /// <param name="pRequestId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<Request> CreateRequestFromPendingRequest(Guid pRequestId, Guid personId);

        /// <summary>
        /// Создание новой заявки с фронта
        /// </summary>
        /// <param name="form">DTO заявки с данными о потенциальном студенте.</param>
        /// <returns></returns>
        Task CreateRequestFromClient(NewRequestDTO form);

        /// <summary>
        /// Обновить объект.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        Task UpdateRequestData(Guid id, RequestsDTO form);


        /// <summary>
        /// Пагинация заявок
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="filters"></param>
        /// <returns>Пагинированные DTO заявок.</returns>
        Task<PagedPage<RequestsDTO>> GetRequestsDTOByPage(int page, int pageSize, RequestFilterDTO filters);

    }
}