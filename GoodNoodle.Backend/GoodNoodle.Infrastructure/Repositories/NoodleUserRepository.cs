using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GoodNoodle.Infrastructure.Repositories;

public class NoodleUserRepository : Repository<NoodleUser>, INoodleUserRepository
{
    public NoodleUserRepository(GoodNoodleContext context) : base(context)
    {
    }

    public async Task<NoodleUser> GetByNameAsync(string fullName)
    {
        var user = await _dbSet.FirstOrDefaultAsync(x => x.FullName == fullName);
        return user;
    }

    public async Task<NoodleUser> GetByEmailAsync(string email)
    {
        var user = await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        return user;
    }

    public async Task<NoodleUser> GetByGroupAsync(Guid groupId)
    {
        var user = await _dbSet.FirstOrDefaultAsync();
        return user;
    }
}
