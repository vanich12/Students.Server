﻿using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Models;

namespace Students.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс репозитория заявки.
/// </summary>
public interface IRequestRepository : IGenericRepository<Request>
{
  /// <summary>
  /// Добавление приказа в заявку.
  /// </summary>
  /// <param name="requestId">Идентификатор заявки.</param>
  /// <param name="order">Приказ.</param>
  /// <returns>Идентификатор заявки.</returns>
  Task<Guid?> AddOrderToRequest(Guid requestId, Order order);

    /// <summary>
    /// Список заявок, в которые подавал студент.
    /// </summary>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>Список заявок.</returns>
    Task<IEnumerable<RequestsDTO>?> GetListRequestsOfStudentExists(Guid studentId);

  /// <summary>
  /// Пагинация заявок.
  /// </summary>
  /// <param name="page">Номер страницы.</param>
  /// <param name="pageSize">Размер страницы.</param>
  /// <returns>Пагинированные DTO заявок.</returns>
  Task<PagedPage<RequestsDTO>> GetRequestsDTOByPage(int page, int pageSize, RequestFilterDTO filters);
}