using GoodNoodle.Application.ViewModel.NoodleUser;
using MediatR;
using System;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.UserInGroup;

public class GetUserInGroupsByGroupQuery : IRequest<List<NoodleUserInGroupViewModel>>
{
    public Guid GroupId { get; set; }

    public GetUserInGroupsByGroupQuery(Guid groupId)
    {
        GroupId = groupId;
    }
}
