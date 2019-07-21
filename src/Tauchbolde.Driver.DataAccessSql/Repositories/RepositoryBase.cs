using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    /// <summary>
    /// Implementation of the repository base class.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity.</typeparam>
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
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var entityEntry = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            
            return entityEntry.Entity;
        }

        /// <inheritdoc />
        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Context.Update(entity);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
