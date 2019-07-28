using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    internal abstract class RepositoryBase<TEntity, TDataEntity> : IRepository<TEntity>
        where TEntity: class, new()
        where TDataEntity: class, new()
    {
        protected readonly ApplicationDbContext Context;

        protected RepositoryBase(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindByIdAsync(Guid id) =>
            MapTo(
                await Context.Set<TDataEntity>().FindAsync(id));

        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> GetAllAsync() =>
            (await Context.Set<TDataEntity>().ToListAsync())
            .Select(MapTo)
            .ToList();

        /// <inheritdoc />
        public virtual async Task<TEntity> InsertAsync([NotNull] TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var entityEntry = await Context.AddAsync(MapTo(entity));
            await Context.SaveChangesAsync();
            
            return MapTo(entityEntry.Entity);
        }

        /// <inheritdoc />
        public virtual async Task UpdateAsync([NotNull] TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Context.Update(MapTo(entity));
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync([NotNull] TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            Context.Set<TDataEntity>().Remove(MapTo(entity));
            await Context.SaveChangesAsync();
        }

        protected abstract TEntity MapTo(TDataEntity dataEntity);
        protected abstract TDataEntity MapTo(TEntity domainEntity);
    }
}
