using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Infrastructure.Database;

namespace GoodNoodle.Infrastructure.Repositories;
public class InvitationsRepository : Repository<Invitations>, IInvitationsRepository
{
    public InvitationsRepository(GoodNoodleContext context) : base(context)
    {
    }
}
