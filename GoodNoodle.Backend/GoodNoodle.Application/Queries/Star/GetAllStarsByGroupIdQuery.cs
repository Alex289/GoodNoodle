using GoodNoodle.Application.ViewModel.Star;
using MediatR;
using System;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.Star;

public class GetAllStarsByGroupIdQuery : IRequest<List<StarViewModel>>
{
    public Guid GroupId { get; set; }

    public GetAllStarsByGroupIdQuery(Guid groupId)
    {
        GroupId = groupId;
    }
}
