using GoodNoodle.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.Interfaces.Repositories;

public interface INoodleUserRepository : IRepository<NoodleUser>
{
    Task<NoodleUser> GetByNameAsync(string name);
    Task<NoodleUser> GetByEmailAsync(string email);
    Task<NoodleUser> GetByGroupAsync(Guid groupId);
}
