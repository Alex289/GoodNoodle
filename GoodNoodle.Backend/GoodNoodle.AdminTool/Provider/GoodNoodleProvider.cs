using GoodNoodle.Domain.Entities;
using GoodNoodle.Infrastructure.Database;

namespace GoodNoodle.AdminTool.Provider;

public class GoodNoodleProvider : IGoodNoodleProvider
{
    private readonly GoodNoodleContext _context;

    public GoodNoodleProvider(GoodNoodleContext context)
    {
        _context = context;
    }

    public async Task CreateAdmin(NoodleUser admin)
    {
        _context.Add(admin);
        await _context.SaveChangesAsync();
    }

    public async Task<NoodleUser> GetAdmin(Guid id)
    {
        return await _context.NoodleUser.FindAsync(id);
    }
}
