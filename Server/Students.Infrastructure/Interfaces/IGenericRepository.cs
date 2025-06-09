using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Students.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс generic репозитория.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Создание.
    /// </summary>
    /// <param name="item">Объект.</param>
    /// <returns>Объект.</returns>
    Task<TEntity> Create(TEntity item);

    /// <summary>
    /// Поиск объекта по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Объект.</returns>
    Task<TEntity?> FindById(Guid id);

    /// <summary>
    /// Список объектов.
    /// </summary>
    /// <returns>Список объектов.</returns>
    Task<IEnumerable<TEntity>> GetAll();

    /// <summary>
    /// Список объектов, с указанным условием.
    /// </summary>
    /// <param name="predicate"> условие, предикат</param>
    /// <param name="eagerQuery"> "жадный запрос к бд (нужен в случае, если надо запросить сразу какие-то связные данные)"</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? eagerQuery = null);

    /// <summary>
    /// Получение подходящей сущности.
    /// </summary>
    /// <param name="predicate">Функция, по условию которой производится отбор данных из БД.</param>
    /// <returns>Сущность.</returns>
    Task<TEntity?> GetOne(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Удаление объекта.
    /// </summary>
    /// <param name="item">Объект.</param>
    /// <returns>Результат удаления.</returns>
    Task Remove(TEntity item);

    /// <summary>
    /// Изменение объекта.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="item">Объект.</param>
    /// <returns>Объект.</returns>
    Task<TEntity?> Update(Guid id, TEntity item);

    /// <summary>
    /// Частичное изменение объекта.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="item">Объект.</param>
    /// <returns>Объект.</returns>
    Task<TEntity?> Patch(Guid id, TEntity item);
}