using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNoodle.Infrastructure.Repositories;

public class StarRepository : Repository<Star>, IStarRepository
{
    public StarRepository(GoodNoodleContext context) : base(context)
    {
    }

    public async Task<List<Star>> GetByUserAsync(Guid userId)
    {
        var stars = await _dbSet.Where(x => x.NoodleUserId == userId).ToListAsync();
        return stars;
    }
}
