namespace Tauchbolde.Application.DataGateways
{
    public interface IRepository<TEntity>
        where TEntity: class
    {
        Task<TEntity> FindByIdAsync(Guid id);
        Task<ICollection<TEntity>> GetAllAsync();
        Task<TEntity> InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}