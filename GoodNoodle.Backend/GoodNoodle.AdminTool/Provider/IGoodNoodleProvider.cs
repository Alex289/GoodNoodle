using GoodNoodle.Domain.Entities;

namespace GoodNoodle.AdminTool.Provider;

public interface IGoodNoodleProvider
{
    public Task CreateAdmin(NoodleUser admin);
    public Task<NoodleUser> GetAdmin(Guid id);
}
