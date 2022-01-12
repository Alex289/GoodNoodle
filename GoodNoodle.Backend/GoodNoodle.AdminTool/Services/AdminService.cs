using GoodNoodle.AdminTool.Provider;
using GoodNoodle.Domain.Entities;

namespace GoodNoodle.AdminTool.Services;

public class AdminService : IAdminService
{
    private readonly IGoodNoodleProvider _goodNoodleProvider;

    public AdminService(IGoodNoodleProvider goodNoodleProvider)
    {
        _goodNoodleProvider = goodNoodleProvider;
    }

    public async Task CreateAdmin(NoodleUser admin)
    {
        await _goodNoodleProvider.CreateAdmin(admin);
    }

    public async Task<NoodleUser> GetAdmin(Guid id)
    {
        return await _goodNoodleProvider.GetAdmin(id);
    }
}
