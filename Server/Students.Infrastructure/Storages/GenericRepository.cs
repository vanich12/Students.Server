﻿using Microsoft.EntityFrameworkCore;
using Students.DBCore.Contexts;
using Students.Infrastructure.Interfaces;

namespace Students.Infrastructure.Storages;

/// <summary>
/// Репозиторий Generic.
/// </summary>
/// <typeparam name="TEntity">Сущность, с которой работает репозиторий.</typeparam>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    #region Поля и свойства

    private readonly StudentContext _context;
    private readonly DbSet<TEntity> _dbSet;

    #endregion

    #region Методы

    /// <summary>
    /// Получение списка сущностей с загрузкой из базы данных.
    /// </summary>
    /// <returns>Список сущностей с загрузкой из базы данных.</returns>
    public virtual async Task<IEnumerable<TEntity>> Get()
    {
        return await this._dbSet.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Получение списка сущностей.
    /// </summary>
    /// <param name="predicate">Функция, по условию которой производится отбор данных из БД.</param>
    /// <returns>Список сущностей.</returns>
    public virtual async Task<IEnumerable<TEntity>> Get(Predicate<TEntity> predicate)
    {
        var items = new List<TEntity>();
        await foreach (var item in this._dbSet.AsNoTracking().AsAsyncEnumerable())
        {
            if (predicate(item))
                items.Add(item);
        }

        return items;
    }

    /// <summary>
    /// Получение подходящей сущности.
    /// </summary>
    /// <param name="predicate">Функция, по условию которой производится отбор данных из БД.</param>
    /// <returns>Сущность.</returns>
    public async Task<TEntity?> GetOne(Predicate<TEntity> predicate)
    {
        await foreach (var item in this._dbSet.AsAsyncEnumerable())
        {
            if (predicate(item))
                return item;
        }

        return null;
    }

    /// <summary>
    /// Поиск сущности по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Сущность.</returns>
    public virtual async Task<TEntity?> FindById(Guid id)
    {
        return await this._dbSet.FindAsync(id);
    }

    /// <summary>
    /// Создание сущности.
    /// </summary>
    /// <param name="item">Сущность.</param>
    /// <returns>Сущность.</returns>
    public virtual async Task<TEntity> Create(TEntity item)
    {
        this._dbSet.Add(item);
        await this._context.SaveChangesAsync();
        return item;
    }

    /// <summary>
    /// Изменение сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="item">Обновлённая сущность.</param>
    /// <returns>Сущность.</returns>
    public virtual async Task<TEntity?> Update(Guid id, TEntity item)
    {
        var oldItem = await this._dbSet.FindAsync(id);
        if (oldItem == null)
            return null;

        item.GetType().GetProperty("Id")?.SetValue(item, id);
        this._context.Entry(oldItem).CurrentValues.SetValues(item);
        await this._context.SaveChangesAsync();
        return item;
    }
    /// <summary>
    /// Частичное обновление сущности
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="item">Обновлённая сущность.</param>
    /// <returns></returns>
    public async Task<TEntity?> Patch(Guid id, TEntity item)
    {
        var oldItem = await this._dbSet.FindAsync(id);
        if (oldItem == null)
            return null;

        foreach (var property in item.GetType().GetProperties())
        {
            var oldItemProperty = oldItem.GetType().GetProperty(property.Name);
            if (oldItemProperty is null || !oldItemProperty.CanRead || !oldItemProperty.CanWrite)
                continue;

            object oldValue = oldItemProperty.GetValue(oldItem);
            object newValue = property.GetValue(item);

            if (oldItemProperty.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                continue; 
            
            if (!object.Equals(oldValue, newValue))
                oldItemProperty.SetValue(oldItem, newValue);
        }

        await this._context.SaveChangesAsync();
        return item;
    }

    /// <summary>
    /// Удаление сущности.
    /// </summary>
    /// <param name="item">Сущность.</param>
    /// <returns>Результат удаления.</returns>
    public async Task Remove(TEntity item)
    {
        this._dbSet.Remove(item);
        await this._context.SaveChangesAsync();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public GenericRepository(StudentContext context)
    {
        this._context = context;
        this._dbSet = context.Set<TEntity>();
    }

    #endregion
}