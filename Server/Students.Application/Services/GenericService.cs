using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.Interfaces;

namespace Students.Application.Services
{
    public class GenericService<TEntity>(IGenericRepository<TEntity> repository)
        : IGenericService<TEntity> where TEntity : class
    {
        public async Task<TEntity> Create(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return await repository.Create(item);
        }

        public async Task<TEntity?> FindById(Guid id)
        {
            var item = await repository.FindById(id);
            return item ?? throw new KeyNotFoundException($"Item {id} not found");
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await repository.Get();
        }

        public async Task<IEnumerable<TEntity>> Get(Predicate<TEntity> predicate)
        {
            var result = await repository.Get(predicate);
            return result ?? throw new KeyNotFoundException("Items not found");
        }

        public async Task<TEntity?> GetOne(Predicate<TEntity> predicate)
        {
            return await repository.GetOne(predicate);
        }

        public async Task Remove(TEntity item)
        {
            await repository.Remove(item);
        }

        public async Task<TEntity?> Update(Guid id, TEntity item)
        {
            return await repository.Update(id, item);
        }
    }
}