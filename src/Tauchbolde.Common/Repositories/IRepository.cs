using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Tauchbolde.Common.Repository
{
    public interface IRepository<TEntity>
        where TEntity: class, new()
    {
        Task<TEntity> FindByIdAsync(Guid id);
        Task<ICollection<TEntity>> GetAllAsync();

        EntityEntry<TEntity> Insert(TEntity entity);
        Task<EntityEntry<TEntity>> InsertAsync(TEntity entity);

        EntityEntry<TEntity> Update(TEntity entity);

        void Delete(TEntity entity);
    }
}