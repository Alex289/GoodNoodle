using GoodNoodle.Application.ViewModel.NoodleGroup;
using MediatR;
using System;

namespace GoodNoodle.Application.Queries.NoodleGroup;

public class GetGroupByIdQuery : IRequest<NoodleGroupViewModel>
{
    public Guid GroupId { get; set; }

    public GetGroupByIdQuery(Guid groupId)
    {
        GroupId = groupId;
    }
}
