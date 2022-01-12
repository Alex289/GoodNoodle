using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.Interfaces.Repositories;

public interface IRepository<TEntity>
{
    void Add(TEntity entity);

    IQueryable<TEntity> GetAll();

    ValueTask<TEntity> GetByIdAsync(Guid id);

    Task RemoveAsync(Guid id);

    void Update(TEntity entity);
}
