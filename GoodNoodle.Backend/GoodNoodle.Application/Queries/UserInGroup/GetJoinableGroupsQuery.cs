using GoodNoodle.Application.ViewModel.NoodleGroup;
using MediatR;
using System;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.UserInGroup;

public class GetJoinableGroupsQuery : IRequest<List<NoodleGroupViewModel>>
{
    public Guid UserId { get; set; }

    public GetJoinableGroupsQuery(Guid userId)
    {
        UserId = userId;
    }
}
