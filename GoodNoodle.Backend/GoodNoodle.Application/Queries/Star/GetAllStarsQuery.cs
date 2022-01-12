using GoodNoodle.Application.ViewModel.Star;
using MediatR;
using System.Collections.Generic;

namespace GoodNoodle.Application.Queries.Star;

public class GetAllStarsQuery : IRequest<List<StarViewModel>>
{
}
