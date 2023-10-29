using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Tauchbolde.Application.DataGateways;

namespace Tauchbolde.Driver.DataAccessSql.Repositories
{
    internal abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity: class
    {
        protected readonly ApplicationDbContext Context;

        protected RepositoryBase(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindByIdAsync(Guid id) =>
            await Context.Set<TEntity>().FindAsync(id);

        /// <inheritdoc />
        public virtual async Task<ICollection<TEntity>> GetAllAsync() =>
            (await Context.Set<TEntity>().ToListAsync())
            .ToList();

        /// <inheritdoc />
        public virtual async Task<TEntity> InsertAsync([NotNull] TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var entityEntry = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            
            return entityEntry.Entity;
        }

        /// <inheritdoc />
        public virtual async Task UpdateAsync([NotNull] TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Context.Update(entity);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync([NotNull] TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
