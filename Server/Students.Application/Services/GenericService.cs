using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Logging;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.Interfaces;

namespace Students.Application.Services
{
    /// <summary>
    /// Универсальный сервис
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="repository"></param>
    public class GenericService<TEntity>(IGenericRepository<TEntity> repository, ILogger<TEntity> logger)
        : IGenericService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Создание сущности
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Create(TEntity item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                return await repository.Create(item);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, $"Error while trying create item {item}");
                throw;
            }
        }
        /// <summary>
        /// Найти сущность по id
        /// </summary>
        /// <param name="id">Id сущности</param>
        /// <returns></returns>
        public async Task<TEntity?> FindById(Guid id)
        {
            try
            {
                var item = await repository.FindById(id);
                return item ?? throw new KeyNotFoundException($"Item {id} not found");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, $"Error while trying Find item by Id: {id}");
                throw;
            }
        }
        /// <summary>
        /// Получить список сущностей
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> Get()
        {
            try
            {
                return await repository.GetAll();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, $"Error while trying Get items of {typeof(TEntity)}");
                throw;
            }
        }
        /// <summary>
        /// получить список сущностей, соответсвующих условию (предикату)
        /// </summary>
        /// <param name="predicate">предикат</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = await repository.Get(predicate);
                return result ?? throw new KeyNotFoundException("Items not found");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message,
                    $"Error while trying Get items of {typeof(TEntity)} with predicate: {predicate}");
                throw;
            }
        }
        /// <summary>
        /// Получить сущность,соответсвующую условию (предикату)
        /// </summary>
        /// <param name="predicate">предикат</param>
        /// <returns></returns>
        public async Task<TEntity?> GetOne(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await repository.GetOne(predicate);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message,
                    $"Error while trying Get item of {typeof(TEntity)} with predicate: {predicate}");
                throw;
            }
        }
        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <param name="item">сущность</param>
        /// <returns></returns>
        public async Task Remove(TEntity item)
        {
            try
            {
                await repository.Remove(item);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, $"Error while trying remove item: {item}");
                throw;
            }
        }
        /// <summary>
        /// Обновить сущность 
        /// </summary>
        /// <param name="id">Id сущности</param>
        /// <param name="item">сущность</param>
        /// <returns></returns>
        public async Task<TEntity?> Update(Guid id, TEntity item)
        {
            try
            {
                return await repository.Update(id, item);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message,
                    $"Error while trying update item: {item} by Id: {id} with type {typeof(TEntity)}");
                throw;
            }
        }
    }
}