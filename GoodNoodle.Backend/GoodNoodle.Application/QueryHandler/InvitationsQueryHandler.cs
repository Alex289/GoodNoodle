using GoodNoodle.Application.Queries.Invitations;
using GoodNoodle.Application.ViewModel.Invitations;
using GoodNoodle.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Application.QueryHandler;
public class InvitationsQueryHandler : QueryHandler,
    IRequestHandler<GetInvitationsByGroupQuery, List<InvitationsViewModel>>
{
    private readonly IInvitationsRepository _invitationsRepository;

    public InvitationsQueryHandler(IInvitationsRepository invitationsRepository, IMediator bus) : base(bus)
    {
        _invitationsRepository = invitationsRepository;
    }

    public async Task<List<InvitationsViewModel>> Handle(GetInvitationsByGroupQuery request, CancellationToken cancellationToken)
    {
        return await _invitationsRepository
            .GetAll()
            .Where(x => x.NoodleGroupId == request.NoodleGroupId)
            .Select(item => new InvitationsViewModel
            {
                Id = item.Id,
                NoodleUserId = item.NoodleUserId,
                NoodleGroupId = item.NoodleGroupId,
                Role = item.Role
            }).ToListAsync(cancellationToken);
    }
}
