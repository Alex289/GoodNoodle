using GoodNoodle.Domain.Entities;

namespace GoodNoodle.AdminTool.Services;

public interface IAdminService
{
    public Task CreateAdmin(NoodleUser admin);
    public Task<NoodleUser> GetAdmin(Guid id);
}
