using System.Collections.Generic;
using System.Threading.Tasks;

namespace TPC.Api.Persistence.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(long id);

        Task<IEnumerable<TEntity>> GetAll();

        TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

        void Remove(TEntity entity);
    }
}
