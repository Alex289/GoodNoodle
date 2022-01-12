using GoodNoodle.Application.ViewModel.Star;
using MediatR;
using System;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.Star;

public class GetAllStarsByUserIdQuery : IRequest<List<StarViewModel>>
{
    public Guid UserId { get; set; }

    public GetAllStarsByUserIdQuery(Guid userId)
    {
        UserId = userId;
    }
}
