using GoodNoodle.Application.ViewModel.Invitations;
using MediatR;
using System;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.Invitations;
public class GetInvitationsByGroupQuery : IRequest<List<InvitationsViewModel>>
{
    public Guid NoodleGroupId { get; set; }

    public GetInvitationsByGroupQuery(Guid noodleGroupId)
    {
        NoodleGroupId = noodleGroupId;
    }
}
