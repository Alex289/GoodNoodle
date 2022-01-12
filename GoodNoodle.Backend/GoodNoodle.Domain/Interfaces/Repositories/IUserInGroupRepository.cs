using GoodNoodle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.Interfaces.Repositories;

public interface IUserInGroupRepository : IRepository<UserInGroup>
{
    public Task<List<UserInGroup>> GetByGroupAsync(Guid groupId);
    public Task<List<UserInGroup>> GetByUserAsync(Guid userId);
}
