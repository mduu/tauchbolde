using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tauchbolde.Common.Repositories;

namespace Tauchbolde.DataAccess.Repositories
{
    /// <summary>
    /// Implementation of the repository base class.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity: class, new()
    {
        protected readonly ApplicationDbContext Context;

        protected RepositoryBase(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }
        
        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        /// <inheritdoc />
        public virtual EntityEntry<TEntity> Insert(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return Context.Add(entity);
        }

        /// <inheritdoc />
        public virtual async Task<EntityEntry<TEntity>> InsertAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return await Context.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual EntityEntry<TEntity> Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return Context.Update(entity);
        }

        /// <inheritdoc />
        public virtual void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
    }
}
