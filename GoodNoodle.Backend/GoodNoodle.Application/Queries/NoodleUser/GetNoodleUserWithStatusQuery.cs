using GoodNoodle.Application.ViewModel.NoodleUser;
using MediatR;
using System.Collections.Generic;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Application.Queries.NoodleUser;

public class GetNoodleUserWithStatusQuery : IRequest<List<NoodleUserViewModel>>
{
    public UserStatus Status;

    public GetNoodleUserWithStatusQuery(UserStatus status)
    {
        Status = status;
    }
}
