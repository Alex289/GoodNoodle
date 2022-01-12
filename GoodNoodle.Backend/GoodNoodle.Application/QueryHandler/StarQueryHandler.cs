using GoodNoodle.Application.Queries.Star;
using GoodNoodle.Application.ViewModel.Star;
using GoodNoodle.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace GoodNoodle.Application.QueryHandler;

public class StarQueryHandler : QueryHandler,
    IRequestHandler<GetAllStarsQuery, List<StarViewModel>>,
    IRequestHandler<GetAllStarsByGroupIdQuery, List<StarViewModel>>,
    IRequestHandler<GetAllStarsByUserIdQuery, List<StarViewModel>>
{
    private readonly IStarRepository _starRepository;

    public StarQueryHandler(IStarRepository starRepository, IMediator bus) : base(bus)
    {
        _starRepository = starRepository;
    }

    public async Task<List<StarViewModel>> Handle(GetAllStarsQuery request, CancellationToken cancellationToken)
    {
        List<StarViewModel> starListQuery = await _starRepository.GetAll()
            .Select(item => new StarViewModel
            {
                Id = item.Id,
                UserId = item.NoodleUserId,
                GroupId = item.NoodleGroupId,
                Reason = item.Reason,
            }).ToListAsync(cancellationToken);


        return starListQuery;
    }

    public async Task<List<StarViewModel>> Handle(GetAllStarsByGroupIdQuery request, CancellationToken cancellationToken)
    {
        List<StarViewModel> starListQuery = await _starRepository.GetAll()
            .Where(x => x.NoodleGroupId == request.GroupId)
            .Select(item => new StarViewModel
            {
                Id = item.Id,
                UserId = item.NoodleUserId,
                GroupId = item.NoodleGroupId,
                Reason = item.Reason,
            }).ToListAsync(cancellationToken);

        return starListQuery;
    }

    public async Task<List<StarViewModel>> Handle(GetAllStarsByUserIdQuery request, CancellationToken cancellationToken)
    {
        List<StarViewModel> starListQuery = await _starRepository.GetAll()
            .Where(x => x.NoodleUserId == request.UserId)
            .Select(item => new StarViewModel
            {
                Id = item.Id,
                UserId = item.NoodleUserId,
                GroupId = item.NoodleGroupId,
                Reason = item.Reason,
            }).ToListAsync(cancellationToken);

        return starListQuery;
    }
}
