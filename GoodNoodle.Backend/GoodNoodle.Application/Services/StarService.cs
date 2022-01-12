using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.Queries.Star;
using GoodNoodle.Application.ViewModel.Star;
using GoodNoodle.Domain.Commands.Stars;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Application.Services;

public class StarService : IStarService
{
    private readonly IMediator _bus;

    public StarService(IMediator bus)
    {
        _bus = bus;
    }

    public Task<List<StarViewModel>> GetAllStarsAsync()
    {
        return _bus.Send(new GetAllStarsQuery());
    }

    public Task<List<StarViewModel>> GetAllStarsByGroupIdAsync(Guid groupId)
    {
        return _bus.Send(new GetAllStarsByGroupIdQuery(groupId));
    }

    public Task<List<StarViewModel>> GetAllStarsByUserIdAsync(Guid groupId)
    {
        return _bus.Send(new GetAllStarsByUserIdQuery(groupId));
    }

    public async Task<Guid> CreateStarAsync(StarCreateViewModel star)
    {
        var newStarGuid = Guid.NewGuid();

        await _bus.Send(new CreateStarCommand(
            newStarGuid,
            star.UserId,
            star.GroupId,
            star.Reason));

        return newStarGuid;
    }

    public Task UpdateStarAsync(Guid id, StarUpdateViewModel viewModel)
    {
        return _bus.Send(new UpdateStarCommand(
            id,
            viewModel.Reason));
    }

    public Task RemoveStarAsync(Guid id)
    {
        return _bus.Send(new DeleteStarCommand(id));
    }
}
