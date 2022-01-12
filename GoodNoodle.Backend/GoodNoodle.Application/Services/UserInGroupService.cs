using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.Queries.UserInGroup;
using GoodNoodle.Application.ViewModel.NoodleGroup;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Application.ViewModel.UserInGroup;
using GoodNoodle.Domain.Commands.UserInGroup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Application.Services;

public class UserInGroupService : IUserInGroupService
{
    private readonly IMediator _bus;

    public UserInGroupService(IMediator bus)
    {
        _bus = bus;
    }

    public Task<List<UserInGroupViewModel>> GetAllUserInGroupsAsync()
    {
        return _bus.Send(new GetAllUserInGroupsQuery());
    }

    public Task<List<NoodleGroupViewModel>> GetUserByUserIdAsync(Guid userId)
    {
        return _bus.Send(new GetUserInGroupsByUserQuery(userId));
    }

    public Task<List<NoodleUserInGroupViewModel>> GetUserByGroupIdAsync(Guid groupId)
    {
        return _bus.Send(new GetUserInGroupsByGroupQuery(groupId));
    }

    public Task<List<NoodleGroupViewModel>> GetJoinableGroups(Guid userId)
    {
        return _bus.Send(new GetJoinableGroupsQuery(userId));
    }

    public async Task<Guid> CreateUserInGroupAsync(CreateUserInGroupViewModel userInGroup)
    {
        var newUserInGroupId = Guid.NewGuid();
        await _bus.Send(new CreateUserInGroupCommand(userInGroup.Id, newUserInGroupId));

        return newUserInGroupId;
    }

    public Task InviteUser(InviteUserViewModel inviteUser)
    {
        return _bus.Send(new InviteUserCommand(
            inviteUser.Id,
            inviteUser.FullName,
            inviteUser.Email,
            inviteUser.Role,
            inviteUser.GroupId,
            inviteUser.GroupName));
    }

    public Task UpdateUserInGroupAsync(Guid id, UserInGroupViewModel userInGroup)
    {
        return _bus.Send(new UpdateUserInGroupCommand(
            id,
            userInGroup.NoodleGroupId,
            userInGroup.NoodleUserId,
            userInGroup.Role));
    }

    public Task DeleteUserInGroupAsync(Guid id)
    {
        return _bus.Send(new DeleteUserInGroupCommand(id));
    }
}
