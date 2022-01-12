using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNoodle.Infrastructure.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly GoodNoodleContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(GoodNoodleContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public virtual void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public ValueTask<TEntity> GetByIdAsync(Guid id)
    {
        return _dbSet.FindAsync(id);
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        TEntity entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
}
