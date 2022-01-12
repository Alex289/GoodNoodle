using GoodNoodle.Application.ViewModel.NoodleGroup;
using MediatR;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.NoodleGroup;

public class GetAllNoodleGroupsQuery : IRequest<List<NoodleGroupViewModel>>
{
}
