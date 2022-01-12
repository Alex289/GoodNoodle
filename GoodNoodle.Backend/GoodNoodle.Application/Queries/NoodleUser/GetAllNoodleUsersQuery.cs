using GoodNoodle.Application.ViewModel.NoodleUser;
using MediatR;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.NoodleUser;

public class GetAllNoodleUsersQuery : IRequest<List<NoodleUserViewModel>>
{
}
