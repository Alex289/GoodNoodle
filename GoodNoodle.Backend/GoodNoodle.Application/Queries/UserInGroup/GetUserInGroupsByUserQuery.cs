using GoodNoodle.Application.ViewModel.NoodleGroup;
using MediatR;
using System;
using System.Collections.Generic;
namespace GoodNoodle.Application.Queries.UserInGroup;

public class GetUserInGroupsByUserQuery : IRequest<List<NoodleGroupViewModel>>
{
    public Guid UserId { get; set; }

    public GetUserInGroupsByUserQuery(Guid userId)
    {
        UserId = userId;
    }
}
