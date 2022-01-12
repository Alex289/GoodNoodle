using GoodNoodle.Application.ViewModel.NoodleUser;
using MediatR;

namespace GoodNoodle.Application.Queries.NoodleUser;

public class GetNoodleUserWithNameQuery : IRequest<NoodleUserViewModel>
{
    public string Name;

    public GetNoodleUserWithNameQuery(string name)
    {
        Name = name;
    }
}
