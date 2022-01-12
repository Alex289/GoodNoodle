using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.Queries.Invitations;
using GoodNoodle.Application.ViewModel.Invitations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Application.Services;
public class InvitationsService : IInvitationsService
{
    private readonly IMediator _bus;

    public InvitationsService(IMediator bus)
    {
        _bus = bus;
    }

    public Task<List<InvitationsViewModel>> GetInvitationsByGroup(Guid groupId)
    {
        return _bus.Send(new GetInvitationsByGroupQuery(groupId));
    }
}
