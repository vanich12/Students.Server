using Students.Infrastructure.DTO;
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
}