using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.Queries.NoodleUser;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Commands.NoodleUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Application.Services;

public class NoodleUserService : INoodleUserService
{
    private readonly IMediator _bus;

    public NoodleUserService(IMediator bus)
    {
        _bus = bus;
    }

    public Task<List<NoodleUserViewModel>> GetAllNoodleUsersAsync()
    {
        return _bus.Send(new GetAllNoodleUsersQuery());
    }

    public Task<NoodleUserViewModel> GetNoodleUserByIdAsync(Guid id)
    {
        return _bus.Send(new GetNoodleUserWithIdQuery(id));
    }

    public Task<NoodleUserViewModel> GetNoodleUserByNameAsync(string name)
    {
        return _bus.Send(new GetNoodleUserWithNameQuery(name));
    }

    public Task<NoodleUserViewModel> GetNoodleUserByGroupAsync(Guid groupId)
    {
        return _bus.Send(new GetNoodleUserWithGroupQuery(groupId));
    }

    public Task<List<NoodleUserViewModel>> GetNoodleUserByStatusAsync(UserStatus status)
    {
        return _bus.Send(new GetNoodleUserWithStatusQuery(status));
    }

    public Task<List<NoodleUserViewModel>> SearchNoodleUserByNameAsync(string name)
    {
        return _bus.Send(new SearchNoodleUserWithNameQuery(name));
    }

    public async Task<string> CreateNoodleUser(NoodleUserCreateViewModel noodleUser)
    {
        var newUserGuid = Guid.NewGuid();

        await _bus.Send(new CreateNoodleUserCommand(
            newUserGuid,
            noodleUser.FullName,
            noodleUser.Password,
            noodleUser.Email));

        noodleUser.Id = newUserGuid;

        return await _bus.Send(new LoginNoodleUserCommand(
            noodleUser.Email,
            noodleUser.Password));
    }

    public async Task<string> LoginNoodleUser(NoodleUserLoginViewModel noodleUser)
    {
        return await _bus.Send(new LoginNoodleUserCommand(
            noodleUser.Email,
            noodleUser.Password));
    }

    public Task UpdateNoodleUser(Guid id, NoodleUserUpdateViewModel noodleUser)
    {
        return _bus.Send(new UpdateNoodleUserCommand(
            id,
            noodleUser.FullName,
            noodleUser.Status,
            noodleUser.Email,
            noodleUser.Role));
    }

    public Task ChangePassword(NoodleUserChangePasswordViewModel changePasswordViewModel)
    {
        return _bus.Send(new ChangePasswordCommand(changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword));
    }

    public Task RemoveNoodleUser(Guid id)
    {
        return _bus.Send(new DeleteNoodleUserCommand(id));
    }
}
