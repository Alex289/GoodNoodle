using GoodNoodle.Application.ViewModel.NoodleUser;
using MediatR;
using System;

namespace GoodNoodle.Application.Queries.NoodleUser;

public class GetNoodleUserWithIdQuery : IRequest<NoodleUserViewModel>
{
    public Guid Id;

    public GetNoodleUserWithIdQuery(Guid id)
    {
        Id = id;
    }
}
