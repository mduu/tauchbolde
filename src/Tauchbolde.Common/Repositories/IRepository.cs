using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Tauchbolde.Common.Repository
{
    public interface IRepository<TEntity>
        where TEntity: class, new()
    {
        Task<TEntity> FindByIdAsync(Guid id);

        EntityEntry<TEntity> Insert(TEntity entity);
        Task<EntityEntry<TEntity>> InsertAsync(TEntity entity);

        EntityEntry<TEntity> Update(TEntity entity);
    }
}