using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Models;

namespace Students.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс репозитория приказов.
/// </summary>
public interface IOrderRepository : IGenericRepository<Order>
{
  /// <summary>
  /// Получить приказы.
  /// </summary>
  /// <returns>Приказы.</returns>
  public Task<IEnumerable<OrderDTO>> GetListOrdersWithStudentAsync();

  /// <summary>
  /// Пагинация приказов
  /// </summary>
  /// <param name="page"></param>
  /// <param name="pageSize"></param>
  /// <returns></returns>
  Task<PagedPage<OrderDTO>> GetOrdersByPage(int page, int pageSize);
}