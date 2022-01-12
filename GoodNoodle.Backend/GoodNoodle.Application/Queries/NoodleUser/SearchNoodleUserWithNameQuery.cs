using GoodNoodle.Application.ViewModel.NoodleUser;
using MediatR;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.NoodleUser;

public class SearchNoodleUserWithNameQuery : IRequest<List<NoodleUserViewModel>>
{
    public string Name;

    public SearchNoodleUserWithNameQuery(string name)
    {
        Name = name;
    }
}
