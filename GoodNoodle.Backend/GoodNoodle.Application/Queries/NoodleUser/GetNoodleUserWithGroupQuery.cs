using GoodNoodle.Application.ViewModel.NoodleUser;
using MediatR;
using System;

namespace GoodNoodle.Application.Queries.NoodleUser;

public class GetNoodleUserWithGroupQuery : IRequest<NoodleUserViewModel>
{
    public Guid GroupId;

    public GetNoodleUserWithGroupQuery(Guid groupId)
    {
        GroupId = groupId;
    }
}
