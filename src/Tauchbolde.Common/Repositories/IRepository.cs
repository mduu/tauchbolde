using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tauchbolde.Common.Repository
{
    public interface IRepository<TEntity>
        where TEntity: class, new()
    {
        Task<TEntity> GetById(Guid id);
    }
}