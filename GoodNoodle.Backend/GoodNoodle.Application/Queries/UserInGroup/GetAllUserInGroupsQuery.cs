using GoodNoodle.Application.ViewModel.UserInGroup;
using MediatR;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.UserInGroup;

public class GetAllUserInGroupsQuery : IRequest<List<UserInGroupViewModel>>
{
}
