using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNoodle.Infrastructure.Repositories;

public class UserInGroupRepository : Repository<UserInGroup>, IUserInGroupRepository
{
    public UserInGroupRepository(GoodNoodleContext context) : base(context)
    {
    }

    public async Task<List<UserInGroup>> GetByGroupAsync(Guid groupId)
    {
        var userInGroupList = await _dbSet.Where(x => x.NoodleGroupId == groupId).ToListAsync();

        return userInGroupList;
    }

    public async Task<List<UserInGroup>> GetByUserAsync(Guid userId)
    {
        var userInGroupList = await _dbSet.Where(x => x.NoodleUserId == userId).ToListAsync();

        return userInGroupList;
    }
}
