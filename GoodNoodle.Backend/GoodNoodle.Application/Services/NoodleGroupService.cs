using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.Queries.NoodleGroup;
using GoodNoodle.Application.ViewModel.NoodleGroup;
using GoodNoodle.Domain.Commands.NoodleGroup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Application.Services;

class NoodleGroupService : INoodleGroupService
{
    private readonly IMediator _bus;

    public NoodleGroupService(IMediator bus)
    {
        _bus = bus;
    }

    public Task<List<NoodleGroupViewModel>> GetAllGroupsAsync()
    {
        return _bus.Send(new GetAllNoodleGroupsQuery());
    }

    public Task<NoodleGroupViewModel> GetGroupByIdAsync(Guid groupId)
    {
        return _bus.Send(new GetGroupByIdQuery(groupId));
    }

    public async Task<Guid> CreateNoodleGroup(NoodleGroupCreateViewModel noodleGroup)
    {
        var newGroupId = Guid.NewGuid();

        await _bus.Send(
            new CreateNoodleGroupCommand(
                newGroupId,
                noodleGroup.Name,
                noodleGroup.Image));

        return newGroupId;
    }

    public Task UpdateNoodleGroup(Guid id, NoodleGroupUpdateViewModel noodlegroup)
    {
        return _bus.Send(new UpdateNoodleGroupCommand(id, noodlegroup.Name, noodlegroup.Image));
    }

    public Task DeleteNoodleGroup(Guid id)
    {
        return _bus.Send(new DeleteNoodleGroupCommand(id));
    }
}
