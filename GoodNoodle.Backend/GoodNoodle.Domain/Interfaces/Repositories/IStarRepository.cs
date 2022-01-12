using GoodNoodle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.Interfaces.Repositories;

public interface IStarRepository : IRepository<Star>
{
    public Task<List<Star>> GetByUserAsync(Guid userId);
}
