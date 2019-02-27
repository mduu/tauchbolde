using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tauchbolde.Common.Model;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Common.DomainServices.Repositories;

namespace Tauchbolde.Common.DataAccess
{
    /// <summary>
    /// Implementation of the repository base class.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity: class, new()
    {
        protected readonly ApplicationDbContext Context;

        public RepositoryBase(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }
        
        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public EntityEntry<TEntity> Insert(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return Context.Add(entity);
        }

        public async Task<EntityEntry<TEntity>> InsertAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return await Context.AddAsync(entity);
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return Context.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
    }
}
