using GoodNoodle.Application.Queries.NoodleGroup;
using GoodNoodle.Application.ViewModel.NoodleGroup;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Application.QueryHandler;

public class NoodleGroupQueryHandler : QueryHandler,
    IRequestHandler<GetAllNoodleGroupsQuery, List<NoodleGroupViewModel>>,
    IRequestHandler<GetGroupByIdQuery, NoodleGroupViewModel>
{
    private readonly INoodleGroupRepository _noodleGroupRepository;

    public NoodleGroupQueryHandler(INoodleGroupRepository noodleGroupRepository, IMediator bus) : base(bus)
    {
        _noodleGroupRepository = noodleGroupRepository;
    }

    public async Task<List<NoodleGroupViewModel>> Handle(GetAllNoodleGroupsQuery request, CancellationToken cancellationToken)
    {
        List<NoodleGroupViewModel> noodleUserListQuery = await _noodleGroupRepository.GetAll()
            .Select(item => new NoodleGroupViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Image = item.Image,
            })
            .ToListAsync(cancellationToken);

        return noodleUserListQuery;
    }

    public async Task<NoodleGroupViewModel> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var noodleUserQuery = await _noodleGroupRepository.GetAll()
            .Where(item => item.Id == request.GroupId)
            .Select(item => new NoodleGroupViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Image = item.Image,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (noodleUserQuery == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"Group {request.GroupId} not found");
        }

        return noodleUserQuery;
    }
}
