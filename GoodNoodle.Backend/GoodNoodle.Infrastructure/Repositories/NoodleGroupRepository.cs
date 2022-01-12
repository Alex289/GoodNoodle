using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Infrastructure.Database;

namespace GoodNoodle.Infrastructure.Repositories;

public class NoodleGroupRepository : Repository<NoodleGroup>, INoodleGroupRepository
{
    public NoodleGroupRepository(GoodNoodleContext context) : base(context)
    {
    }
}
